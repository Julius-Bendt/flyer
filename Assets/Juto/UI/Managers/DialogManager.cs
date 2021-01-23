using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Juto.UI
{
    public class DialogManager : MonoBehaviour
    {

        public void ShowDialog(string title, string body, Color color)
        {
            //Fills the whole screen
        }

        public void ShowToast(string title, string body, string time)
        {

        }

        public void ShowSnackbar(string title, string body, Action action)
        {
            action.Invoke();
        }

        public void ShowAlert(string title, string body)
        {

        }

    }

}
