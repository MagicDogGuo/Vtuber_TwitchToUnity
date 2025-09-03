using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MessageEffect : MonoBehaviour
{
 

    [SerializeField]
    Text txt;

    [SerializeField]
    GameObject smoke;

    [SerializeField]
    GameObject[] animal;

    [SerializeField]
    GameObject head;

    [SerializeField]
    GameObject particle;

    [SerializeField]
    Light _light;

    [SerializeField]
    Animator eveAni;

    float lightCounter=0;
    TwitchChat twitchChat;


    // Start is called before the first frame update
    void Start()
    {
        twitchChat = TwitchChat.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        Twitch();
        Test();
    }

    void Twitch()
    {
        //Debug.Log("twitchChat.TwitchClientAvailable:" + twitchChat.TwitchClientAvailable
        //+ "Message:" + twitchChat.Message);
        txt.text = twitchChat._Author + "\n: " + twitchChat._Message ;

        if (twitchChat.TwitchClientAvailable > 0)
        {
            if (twitchChat._Message == "fire")
            {
                Instantiate(particle);
            }
            if (twitchChat._Message == "smoke")
            {
                Instantiate(smoke);
            }
            if (twitchChat._Message == "ani")
            {
                for (int i = 0; i < 3; i++)
                {
                    int temp = Random.Range(0, animal.Length);
                    int tempPosX = Random.Range(-5, 5);
                    int tempPosY = Random.Range(-3, 4);
                    Vector3 appearPos = new Vector3(tempPosX, 5, tempPosY);

                    GameObject ob = Instantiate(animal[temp], appearPos, Quaternion.identity);
                    ob.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                    ob.transform.eulerAngles = new Vector3(tempPosX * 10, 180, tempPosY * 5);
                }
            }

            if (twitchChat._Message == "head")
            {
                head.transform.localScale *= 2;
            }

            if(twitchChat._Message == "light")
            {
                _light.intensity = 0.03f;
            }
            if (_light.intensity == 0.03f)
            {
                lightCounter += Time.deltaTime;
                if (lightCounter > 6)
                {
                    _light.intensity = 1f;
                    lightCounter = 0;
                }
            }


            if (twitchChat._Message == "eve")
            {
                eveAni.SetTrigger("appear");
            }
        }
    }

    void Test()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Instantiate(smoke);

        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            for(int i = 0; i < 3; i++)
            {
                int temp = Random.Range(0, animal.Length);
                int tempPosX = Random.Range(-5, 5);
                int tempPosY = Random.Range(-3, 4);
                Vector3 appearPos = new Vector3(tempPosX, 5, tempPosY);

                GameObject ob = Instantiate(animal[temp], appearPos, Quaternion.identity);
                ob.transform.localScale = new Vector3(0.3f, 0.3f, 0.3f);
                ob.transform.eulerAngles = new Vector3(tempPosX * 10, 180, tempPosY * 5);
            }
          
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            head.transform.localScale *= 2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            head.transform.localScale =Vector3.one;
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            _light.intensity = 0.03f;
        }

        if(_light.intensity == 0.03f)
        {
            lightCounter += Time.deltaTime;
            if (lightCounter > 6)
            {
                _light.intensity = 1f;
                lightCounter = 0;
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            eveAni.SetTrigger("appear");
        }
    }


}
