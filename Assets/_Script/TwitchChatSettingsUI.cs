using UnityEngine;
using UnityEngine.UI;


public class TwitchChatSettingsUI : MonoBehaviour
{
    public InputField PasswordInput;
    public InputField UsernameInput;
    public InputField ChannelNameInput;
    public TwitchChat TwitchChat;


    void Start(){
        // 从PlayerPrefs加载保存的OAuth令牌，如果没有则使用默认值
        if(PlayerPrefs.HasKey("TwitchOAuthPass")){
            var password = PlayerPrefs.GetString("TwitchOAuthPass");
            PasswordInput.text = password;
        } else {
            // 默认值，用户需要在UI中手动输入
            PasswordInput.text = "oauth:your_oauth_token_here";
        }

        // 加载其他保存的设置
        if(PlayerPrefs.HasKey("TwitchUsername")){
            UsernameInput.text = PlayerPrefs.GetString("TwitchUsername");
        }
        
        if(PlayerPrefs.HasKey("TwitchChannel")){
            ChannelNameInput.text = PlayerPrefs.GetString("TwitchChannel");
        }
    }

    public void Connect(){
        // 保存设置到PlayerPrefs
        PlayerPrefs.SetString("TwitchOAuthPass", PasswordInput.text);
        PlayerPrefs.SetString("TwitchUsername", UsernameInput.text);
        PlayerPrefs.SetString("TwitchChannel", ChannelNameInput.text);
        PlayerPrefs.Save();

        TwitchCredentials credentials = new TwitchCredentials{
            ChannelName = ChannelNameInput.text.ToLower(),
            Username = UsernameInput.text.ToLower(),
            Password = PasswordInput.text
        };
        TwitchChat.Connect(credentials, new CommandCollection());
    }

}
