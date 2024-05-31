using Azure;
using Azure.AI.OpenAI;

namespace AI
{
    public class ChatGPT
    {
        /// <summary>
        /// OpenAIの通信用機能
        /// </summary>
        private OpenAIClient gptClient = new OpenAIClient("任意のAPIキーをここに設定してください");
        //private OpenAIClient gptClient = new OpenAIClient("bs-avgDLagMBQGbikGkaB1YT1BlokFJpEa2NjiA5N1TgPiPMrkR");　こんな感じ

        /// <summary>
        /// 対話用設定オプション
        /// </summary>
        private ChatCompletionsOptions options = new ChatCompletionsOptions()
        {
            // ChatGPTのバージョンなど
            DeploymentName = "gpt-3.5-turbo-0125",
            ChoiceCount = 1,
            MaxTokens = 16384,
        };

        /// <summary>
        /// 機能を使う際に設定される処理
        /// </summary>
        public ChatGPT()
        {
            // ここに自分の名前
            string userName = "";

            // AIの特徴を指定するためのシステムメッセージのリストを作ります。
            // これをChatGPTに送るとこの特徴を掴んで返答してくれます。
            List<string> systemMessageList = new List<string>();
            systemMessageList.Add("あなたの名前は「きずだらけ あい」です。");
            systemMessageList.Add(userName + "によって作成されたAIです。");
            systemMessageList.Add(userName + "のAIアシスタントです。");

            // foreachは繰り返し処理です。対話用設定オプションのメッセージリストに先ほど作成した特徴を追加します。
            foreach (var systemMessage in systemMessageList)
            {
                options.Messages.Add(new ChatRequestSystemMessage(systemMessage));
            }
        }

        /// <summary>
        /// AIと会話する処理
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<Chat> ChatAsync(Chat chat)
        {
            // 処理に失敗した場合などを考慮し、例外を取得できるようtry catchを使用する。Exeptionが発生した場合はcatchの処理が実行されます。
            try
            {
                // ユーザーのメッセージを対話用設定オプションのメッセージリストに追加します。
                options.Messages.Add(new ChatRequestUserMessage(chat.Message));

                // 通信機能の中の対話する関数GetChatCompletionsAsyncを使い返答をもらう。
                Response<ChatCompletions> response = await gptClient.GetChatCompletionsAsync(options);

                // 通信した結果Response<ChatCompletions>は色々な情報を持っている。対話の返答はこんな感じで取得する
                string responseMessage = response.Value.Choices[0].Message.Content;

                // 再度メッセージを送るときに以前の会話の内容を送るため対話用設定オプションにAIからのメッセージを追加しておく
                options.Messages.Add(new ChatRequestAssistantMessage(responseMessage));

                // 返却の型をChatオブジェクトとしているので、Chatオブジェクトを作成しName、Messageを設定しChatオブジェクトを返す
                return new Chat() { Name = "AI", Message = responseMessage };
            }
            catch (Exception ex)
            {
                // 例外が発生した場合は下記メッセージを設定したChatオブジェクトを返す。
                Chat responseChat = new Chat();
                responseChat.Name = "AI";
                responseChat.Message = "通信に失敗しました。APIキーなどの見直しを行ってください。";

                return responseChat;
            }
        }
    }

}
