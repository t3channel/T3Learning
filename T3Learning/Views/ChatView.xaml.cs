using AI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace T3Learning.Views
{
    /// <summary>
    /// ChatView.xaml の相互作用ロジック
    /// </summary>
    public partial class ChatView : Page
    {
        private ChatGPT chatGPT = new ChatGPT();

        public ChatView()
        {
            InitializeComponent();
        }

        /// <summary>
        /// キーボードを押した時に動く関数（イベント）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void UserInputTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            // エンターキーを押したかif文で判断する
            // KeyEventArgsのeに押したKeyの情報が入っている
            if (e.Key == Key.Enter)
            {
                string message = UserInputTextBox.Text;

                // ユーザーのメッセージ表示コントロールのテキストに入力したテキストを追加する
                UserChatTextBlock.Text = UserChatTextBlock.Text + message;

                // 入力するコントロールのテキストを空にする
                UserInputTextBox.Text = string.Empty;

                // ChatGPTを使用する際の引数を作成する
                Chat chat = new Chat()
                {
                    Name = "User",
                    Message = message,
                };

                // ChatGPTの機能を使用してAIと会話する
                var aiChat = await chatGPT.ChatAsync(chat);

                // AIのメッセージを取得する。既に会話済みの場合改行を追加
                string aiMessage = string.Empty;
                if (AIChatTextBlock.Text.Length > 0)
                {
                    aiMessage = Environment.NewLine + aiChat.Message;
                }
                else
                {
                    aiMessage = aiChat.Message;
                }

                // AIのメッセージ表示コントロールのテキストに取得したメッセージを追加する
                AIChatTextBlock.Text += aiMessage;
            }
        }
    }
}
