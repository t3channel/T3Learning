using Azure;
using Azure.AI.OpenAI;

namespace AI
{
    public class ChatGPT
    {
        /// <summary>
        /// OpenAI�̒ʐM�p�@�\
        /// </summary>
        private OpenAIClient gptClient = new OpenAIClient("�C�ӂ�API�L�[�������ɐݒ肵�Ă�������");
        //private OpenAIClient gptClient = new OpenAIClient("bs-avgDLagMBQGbikGkaB1YT1BlokFJpEa2NjiA5N1TgPiPMrkR");�@����Ȋ���

        /// <summary>
        /// �Θb�p�ݒ�I�v�V����
        /// </summary>
        private ChatCompletionsOptions options = new ChatCompletionsOptions()
        {
            // ChatGPT�̃o�[�W�����Ȃ�
            DeploymentName = "gpt-3.5-turbo-0125",
            ChoiceCount = 1,
            MaxTokens = 16384,
        };

        /// <summary>
        /// �@�\���g���ۂɐݒ肳��鏈��
        /// </summary>
        public ChatGPT()
        {
            // �����Ɏ����̖��O
            string userName = "";

            // AI�̓������w�肷�邽�߂̃V�X�e�����b�Z�[�W�̃��X�g�����܂��B
            // �����ChatGPT�ɑ���Ƃ��̓�����͂�ŕԓ����Ă���܂��B
            List<string> systemMessageList = new List<string>();
            systemMessageList.Add("���Ȃ��̖��O�́u�������炯 �����v�ł��B");
            systemMessageList.Add(userName + "�ɂ���č쐬���ꂽAI�ł��B");
            systemMessageList.Add(userName + "��AI�A�V�X�^���g�ł��B");

            // foreach�͌J��Ԃ������ł��B�Θb�p�ݒ�I�v�V�����̃��b�Z�[�W���X�g�ɐ�قǍ쐬����������ǉ����܂��B
            foreach (var systemMessage in systemMessageList)
            {
                options.Messages.Add(new ChatRequestSystemMessage(systemMessage));
            }
        }

        /// <summary>
        /// AI�Ɖ�b���鏈��
        /// </summary>
        /// <param name="message"></param>
        /// <returns></returns>
        public async Task<Chat> ChatAsync(Chat chat)
        {
            // �����Ɏ��s�����ꍇ�Ȃǂ��l�����A��O���擾�ł���悤try catch���g�p����BExeption�����������ꍇ��catch�̏��������s����܂��B
            try
            {
                // ���[�U�[�̃��b�Z�[�W��Θb�p�ݒ�I�v�V�����̃��b�Z�[�W���X�g�ɒǉ����܂��B
                options.Messages.Add(new ChatRequestUserMessage(chat.Message));

                // �ʐM�@�\�̒��̑Θb����֐�GetChatCompletionsAsync���g���ԓ������炤�B
                Response<ChatCompletions> response = await gptClient.GetChatCompletionsAsync(options);

                // �ʐM��������Response<ChatCompletions>�͐F�X�ȏ��������Ă���B�Θb�̕ԓ��͂���Ȋ����Ŏ擾����
                string responseMessage = response.Value.Choices[0].Message.Content;

                // �ēx���b�Z�[�W�𑗂�Ƃ��ɈȑO�̉�b�̓��e�𑗂邽�ߑΘb�p�ݒ�I�v�V������AI����̃��b�Z�[�W��ǉ����Ă���
                options.Messages.Add(new ChatRequestAssistantMessage(responseMessage));

                // �ԋp�̌^��Chat�I�u�W�F�N�g�Ƃ��Ă���̂ŁAChat�I�u�W�F�N�g���쐬��Name�AMessage��ݒ肵Chat�I�u�W�F�N�g��Ԃ�
                return new Chat() { Name = "AI", Message = responseMessage };
            }
            catch (Exception ex)
            {
                // ��O�����������ꍇ�͉��L���b�Z�[�W��ݒ肵��Chat�I�u�W�F�N�g��Ԃ��B
                Chat responseChat = new Chat();
                responseChat.Name = "AI";
                responseChat.Message = "�ʐM�Ɏ��s���܂����BAPI�L�[�Ȃǂ̌��������s���Ă��������B";

                return responseChat;
            }
        }
    }

}
