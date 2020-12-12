using System;
using System.Text;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using CefSharp.Handler;
using Newtonsoft.Json;

namespace Classcard_Hack
{
    public partial class Client : Form
    {
        public string CurrentAddress { get; set; }
        public string[] CorrectAnswers { get; set; }

        private ChromiumWebBrowser chromiumBrowser;

        public Client()
        {
            InitializeComponent();
            InitializeChromium();
        }

        private void Client_Load(object sender, EventArgs e)
        {

        }

        private void InitializeChromium()
        {
            Cef.Initialize(new CefSettings() { LogSeverity = LogSeverity.Disable });

            chromiumBrowser = new ChromiumWebBrowser("http://www.classcard.net/")
            {
                Dock = DockStyle.Fill,
                RequestHandler = new CustomRequestHandler(this)
            };

            chromiumBrowser.AddressChanged += AddressChangedEventHandler;
            chromiumBrowser.LoadingStateChanged += LoadingStateChangedEventHandler;
            chromiumBrowser.ConsoleMessage += ConsoleMessageEventHandler;

            this.Controls.Add(chromiumBrowser);
        }

        #region BrowserEventHandlers
        private void AddressChangedEventHandler(object sender, AddressChangedEventArgs e)
        {
            this.CurrentAddress = e.Address;
        }
        private void LoadingStateChangedEventHandler(object sender, LoadingStateChangedEventArgs e)
        {
            if (!e.IsLoading)
            {
                string[] target = { ".adsbygoogle", "#news_div" };
                string script = string.Format("document.querySelectorAll('{0}').forEach(el => el.parentElement.removeChild(el));", string.Join(",", target));
                chromiumBrowser.ExecuteScriptAsync(script);
            }
        }
        private void ConsoleMessageEventHandler(object sender, ConsoleMessageEventArgs e)
        {
            if (CurrentAddress.Contains("http://www.classcard.net/ClassTest/"))
            {
                if (e.Message.Contains("setAnswer"))
                {
                    string script = @"
                    (function() {
                        var result = new Array();
                        var questions = document.querySelectorAll('.flip-card-back');
                        questions.forEach((question, idx) => {
                            var objective = question.querySelector('.cc-radio-box-body');
                            var subjective = question.querySelector('.subject-input');
                            var answer = question.querySelector('.answer-body');
                            if (objective) {
                                var choices = question.querySelectorAll('.cc-radio-box');
                                var answerText = answer.querySelector('.text-success').innerText;
                                if (answerText) {
                                    choices.forEach(choice => {
                                        if (choice.querySelector('.cc-table>div').innerText == answerText)
                                            result[idx] = choice.querySelector('input').value;
                                    });
                                }
                                else result[idx] = null;
                            }
                            else if (subjective) {
                                var answerText = answer.querySelector('.text-success').innerText;
                                if (answerText) result[idx] = answerText;
                                else result[idx] = null;
                            }
                        });
                        return JSON.stringify(result);
                    })();";

                    chromiumBrowser.EvaluateScriptAsync(script).ContinueWith(x =>
                    {
                        var response = x.Result;
                        if (response.Success && response.Result != null)
                        {
                            var json = response.Result.ToString();
                            CorrectAnswers = JsonConvert.DeserializeObject<string[]>(json);
                        }
                    });
                }
            }
        }
        #endregion BrowserEventHandlers
    }

    #region CustomRequestHandlers
    public class CustomRequestHandler : RequestHandler
    {
        private readonly Client clientForm;

        public CustomRequestHandler(Client client) : base()
        {
            clientForm = client;
        }

        protected override IResourceRequestHandler GetResourceRequestHandler(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, bool isNavigation, bool isDownload, string requestInitiator, ref bool disableDefaultHandling)
        {
            if (request.Method == "POST")
            {
                switch (request.Url)
                {
                    case "http://www.classcard.net/ViewSetAsync/learnAll":
                        return new CustomResourceRequestHandler(clientForm, "LEARN");
                    case "http://www.classcard.net/Match/save":
                        return new CustomResourceRequestHandler(clientForm, "MATCH");
                    case "http://www.classcard.net/ClassTest/submittest":
                        return new CustomResourceRequestHandler(clientForm, "TEST");

                    default:
                        break;
                }
            }

            return null;
        }
    }
    public class CustomResourceRequestHandler : ResourceRequestHandler
    {
        private readonly Client client;
        private readonly string mode;

        public CustomResourceRequestHandler(Client client, string mode) : base()
        {
            this.client = client;
            this.mode = mode;
        }

        protected override CefReturnValue OnBeforeResourceLoad(IWebBrowser chromiumWebBrowser, IBrowser browser, IFrame frame, IRequest request, IRequestCallback callback)
        {
            if (request.PostData.Elements.Count > 0)
            {
                byte[] rawBody;
                string[] parsedBody;

                if (mode == "LEARN")
                {
                    rawBody = request.PostData.Elements[0].Bytes;
                    parsedBody = Encoding.ASCII.GetString(rawBody).Split('&');

                    bool isStartIdx = true;
                    for (int i = 0; i < parsedBody.Length; ++i)
                    {
                        if (parsedBody[i] == "score%5B%5D=0")
                        {
                            if (isStartIdx)
                            {
                                isStartIdx = false;
                            }
                            else
                            {
                                parsedBody[i] = "score%5B%5D=1";
                            }
                        }
                    }

                    rawBody = Encoding.ASCII.GetBytes(string.Join("&", parsedBody));
                    request.PostData.Elements[0].Bytes = rawBody;
                }
                else if (mode == "MATCH")
                {
                    rawBody = request.PostData.Elements[0].Bytes;
                    parsedBody = Encoding.ASCII.GetString(rawBody).Split('&');

                    for (int i = 0; i < parsedBody.Length; ++i)
                    {
                        if (parsedBody[i].Contains("score="))
                        {
                            InputDialog dialog = new InputDialog();
                            dialog.StartPosition = FormStartPosition.CenterScreen;
                            if (dialog.ShowDialog() == DialogResult.OK)
                            {
                                parsedBody[i] = "score=" + dialog.InputResult;
                            }
                        }
                    }

                    rawBody = Encoding.ASCII.GetBytes(string.Join("&", parsedBody));
                    request.PostData.Elements[0].Bytes = rawBody;
                }
                else if (mode == "TEST")
                {
                    rawBody = request.PostData.Elements[0].Bytes;
                    parsedBody = Encoding.ASCII.GetString(rawBody).Split(new string[] { "\r\n" }, StringSplitOptions.None);

                    int answerIdx = 0;
                    for (int i = 0; i < parsedBody.Length; i += 4)
                    {
                        if (parsedBody[i + 1].Contains("user_answer[]"))
                        {
                            string answer = client.CorrectAnswers[answerIdx++];
                            if (answer != null)
                            {
                                parsedBody[i + 3] = answer;
                            }
                        }
                    }

                    rawBody = Encoding.ASCII.GetBytes(string.Join("\r\n", parsedBody));
                    request.PostData.Elements[0].Bytes = rawBody;
                }
            }

            return CefReturnValue.Continue;
        }
    }
    #endregion CustomRequestHandlers
}