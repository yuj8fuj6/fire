﻿using Autodesk.Revit.UI;
using System;
using System.Windows;
using System.Windows.Input;
using Microsoft.Web.WebView2.Core;
using Microsoft.Web.WebView2.Wpf;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Collections.Generic;

/// <summary>
/// For Interop between Web UI and Revit
/// Author: Bob Lee
/// </summary>

namespace fire_rvt
{
    public partial class WebWindow : Window
    {
        private LaunchWeb launch_web;
        public UIApplication uiApp;
        public bool isLoaded = false;
        public WebView2 web_view;
        public List<double[]> exts_to_place;
        public string level_id;
        public string room_id;

        public WebWindow(UIApplication app)
        {
            InitializeComponent();
            launch_web = new LaunchWeb(app, webView);
            DataContext = launch_web;
            launch_web.CloseAction = new Action(this.Close);
            uiApp = app;
            web_view = webView;
        }

        internal class WvReceiveAction
        {
            public string action;
            public object payload;
        }

        private void OnWebViewInteraction(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            WvReceiveAction result = null;
            try
            {
                result = JsonConvert.DeserializeObject<WvReceiveAction>(e.WebMessageAsJson);
                Debug.WriteLine(result.action);
            }
            catch (Exception exception)
            {
                Debug.WriteLine(exception);
            }

            if(result == null) { return; };

            switch (result.action)
            {
                case "getFloors":
                    Debug.WriteLine("Getting Floors!");
                    App.rvtHandler.Raise(RevitEventHandler.RevitActionsEnum.GetFloors);
                    break;

                case "getRooms":
                    Debug.WriteLine("Getting Floors!");
                    App.rvtHandler.Raise(RevitEventHandler.RevitActionsEnum.GetRooms);
                    break;

                case "placeExtinguishers":
                    Debug.WriteLine("Placing Extinguishers!");
                    Debug.WriteLine(result.payload);
                    // FIXME: To get current room level!
                    Debug.WriteLine((result.payload).GetType());
                    Newtonsoft.Json.Linq.JObject extdata = result.payload as Newtonsoft.Json.Linq.JObject;
                    Debug.WriteLine("Handling!");
                    Newtonsoft.Json.Linq.JToken level = extdata["level"];
                    Newtonsoft.Json.Linq.JToken room = extdata["room"];
                    Newtonsoft.Json.Linq.JArray exts = extdata["extinguishers"] as Newtonsoft.Json.Linq.JArray;
                    List<double[]> extinguishers = exts.ToObject<List<double[]>>();
                    room_id = room.ToObject<string>();
                    level_id = level.ToObject<string>();
                    exts_to_place = extinguishers;
                    App.rvtHandler.Raise(RevitEventHandler.RevitActionsEnum.PlaceExtinguishers);
                    break;

                default:
                    Debug.WriteLine(result.action);
                    Debug.WriteLine(result.payload);
                    Debug.WriteLine("Unhandled action. Ignoring.");
                    break;
            }
        }

        public async void SendPayload(string fn, string payload)
        {
            /**
             * Instead of getting the calling the function, because React minifies everything,
             * We have to put an EventListener at `document` to listen for events dispatched there
             * The listeners will then call the appropriate function.
             */
            string payloadScript = "document.dispatchEvent(new CustomEvent(\"" + fn + "\", {\"detail\":" + payload + "}))";
            Debug.WriteLine(payloadScript);
            var res1 = await webView.CoreWebView2.ExecuteScriptAsync(payloadScript);
            Debug.WriteLine(res1);
            return;
        }

        private void DragWindow(object sender, MouseButtonEventArgs e)
        {
            try
            {
                DragMove();
            }
            catch { }
        }
    }
}
