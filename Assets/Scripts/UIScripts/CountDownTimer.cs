using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CountDownTimer : MonoBehaviourPunCallbacks
{
    private Text _countDownText;
    private byte _seconds = 33;
    private byte _minutes = 3;

    private byte _currentTime = 213;

    private void Start()
    {
        _countDownText = GetComponent<Text>();
        CountDownProcess();
    }

    IEnumerator CountDownRoutine()
    {
        yield return new WaitForSeconds(1);
        CountDownProcess();
    }
    
    private void CountDownProcess()
    {
        if (_minutes < 0 || _currentTime < 0)
        {
            StopAllCoroutines();
            return;
        }
        _currentTime -= 1;
        _seconds -= 1;
        if (_seconds < 0)
        {
            _minutes -= 1;
            _seconds = 60;
        }

        _countDownText.text = string.Format("{0}:{1}",_minutes.ToString("00"), _seconds.ToString("00"));
        StartCoroutine(CountDownRoutine());
    }

}
