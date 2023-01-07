// Copyright (c) Microsoft Corporation and Contributors.
// Licensed under the MIT License.


using Microsoft.Win32;

using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Media.Imaging;

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text.Json.Serialization;
using static System.Net.Mime.MediaTypeNames;
using System.Reflection.Metadata;

using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage.Streams;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

using JSON;
using Microsoft.UI.Xaml.Shapes;
using static APPH.MainWindow;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace APPH
{
    /// <summary>
    /// An empty window that can be used on its own or navigated to within a Frame.
    /// </summary>
    /// 

    public sealed partial class MainWindow : Window
    {

        public class Lists
        {
            public string source { get; set; }

            public string ApplicationName { get; set; }

            public string ApplicationDetail { get; set; }

            public bool isHide { get; set; }

            public string visible { get; set; }
            public string path { get; set; }
        }


        public static List<Lists> listslist = new List<Lists>();
        public MainWindow()
        {
            #region 初期処理
            this.InitializeComponent();
            #endregion

            #region レジストリの取得
            string registry1 = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\";
            string registry2 = @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\";
            string registry3 = @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\";

            var rootList = new List<Root>();
            RegistryKey registryKey1 = Registry.LocalMachine.OpenSubKey(registry1);
            string[] aryRegistrySubkeys1 = registryKey1.GetSubKeyNames();



            foreach (string subKeyName in aryRegistrySubkeys1)
            {
                RegistryKey rKey = Registry.LocalMachine.OpenSubKey($@"{registry1}{subKeyName}");
                if (rKey != null)
                {
                    if (rKey.GetValue("DisplayName") != null)
                    {
                        Root root = new Root();
                        root.path = $@"HKLM\{registry1}{subKeyName}";
                        if ((string)rKey.GetValue("DisplayIcon") != null)
                        {
                            root.icon = (string)rKey.GetValue("DisplayIcon");
                        }
                        else
                        {
                            root.icon = "";
                        }
                        root.name = (string)rKey.GetValue("DisplayName");
                        root.version = (string)rKey.GetValue("DisplayVersion");
                        root.publisher = (string)rKey.GetValue("Publisher");
                        root.InstallDate = (string)rKey.GetValue("InstallDate");
                        if (rKey.GetValue("SystemComponent") is Int32)
                        {
                            root.isHide = (Int32)rKey.GetValue("SystemComponent") == 1;
                        }
                        else
                        {
                            root.isHide = false;
                        }
                        var jsonData = JsonConvert.SerializeObject(root);
                        rootList.Add(root);
                        rKey.Close();
                    }
                    else
                    {
                        rKey.Close();
                    }
                }
            }

            registryKey1.Close();
            RegistryKey registryKey2 = Registry.LocalMachine.OpenSubKey(registry2);
            string[] aryRegistrySubkeys2 = registryKey2.GetSubKeyNames();



            foreach (string subKeyName in aryRegistrySubkeys2)
            {
                RegistryKey rKey = Registry.LocalMachine.OpenSubKey($@"{registry2}{subKeyName}");
                if (rKey != null)
                {
                    if (rKey.GetValue("DisplayName") != null)
                    {
                        Root root = new Root();
                        root.path = $@"HKLM\{registry2}{subKeyName}";
                        if ((string)rKey.GetValue("DisplayIcon") != null)
                        {
                            root.icon = (string)rKey.GetValue("DisplayIcon");
                        }
                        else
                        {
                            root.icon = "";
                        }
                        root.name = (string)rKey.GetValue("DisplayName");
                        root.version = (string)rKey.GetValue("DisplayVersion");
                        root.publisher = (string)rKey.GetValue("Publisher");
                        root.InstallDate = (string)rKey.GetValue("InstallDate");
                        if (rKey.GetValue("SystemComponent") != null)
                        {
                            if (rKey.GetValue("SystemComponent") is Int32)
                            {
                                root.isHide = true;
                            }
                            else
                            {
                                root.isHide = false;
                            }
                        }
                        else
                        {
                            root.isHide = false;
                        }
                        var jsonData = JsonConvert.SerializeObject(root);
                        rootList.Add(root);
                        rKey.Close();
                    }
                    else
                    {
                        rKey.Close();
                    }
                }
            }

            registryKey2.Close();

            RegistryKey registryKey3 = Registry.CurrentUser.OpenSubKey(registry3);
            string[] aryRegistrySubkeys3 = registryKey3.GetSubKeyNames();



            foreach (string subKeyName in aryRegistrySubkeys3)
            {
                RegistryKey rKey = Registry.CurrentUser.OpenSubKey($@"{registry3}{subKeyName}");
                if (rKey != null)
                {
                    if (rKey.GetValue("DisplayName") != null)
                    {
                        Root root = new Root();
                        root.path = $@"HKCU\{registry3}{subKeyName}";
                        if ((string)rKey.GetValue("DisplayIcon") != null)
                        {
                            root.icon = (string)rKey.GetValue("DisplayIcon");
                        }
                        else
                        {
                            root.icon = "";
                        }
                        root.name = (string)rKey.GetValue("DisplayName");
                        root.version = (string)rKey.GetValue("DisplayVersion");
                        root.publisher = (string)rKey.GetValue("Publisher");
                        root.InstallDate = (string)rKey.GetValue("InstallDate");
                        if (rKey.GetValue("SystemComponent") is Int32)
                        {
                            root.isHide = (Int32)rKey.GetValue("SystemComponent") == 1;
                        }
                        else
                        {
                            root.isHide = false;
                        }
                        var jsonData = JsonConvert.SerializeObject(root);
                        rootList.Add(root);
                        rKey.Close();
                    }
                    else
                    {
                        rKey.Close();
                    }
                }
            }

            registryKey3.Close();
            rootList.Sort((a, b) => string.Compare(a.name, b.name));
            var rootArray = JsonConvert.SerializeObject(rootList.ToArray());
            Debug.WriteLine(rootArray);
            #endregion

            #region 生成
            var tmpList = new List<string>();
            foreach (Root root in rootList)
            {
                Debug.WriteLine(root.name);
                Lists lists= new Lists();
                lists.ApplicationName = (string)root.name;
                var DetailList = new List<string>();
                if (root.version != null)
                {
                    DetailList.Add(root.version);
                }
                if(root.publisher != null)
                {
                    DetailList.Add(root.publisher);
                }  
                if(root.InstallDate!= null)
                {
                    DetailList.Add($"{root.InstallDate.Substring(0, 4)}/{root.InstallDate.Substring(4, 2)}/{root.InstallDate.Substring(6, 2)}");
                }
                lists.ApplicationDetail = string.Join(" | ",DetailList);
                if (root.isHide)
                {
                    lists.isHide = false;
                    lists.visible = "非表示";
                }
                else
                {
                    lists.isHide = true;
                    lists.visible = "表示";
                }

                try
                {
                    if (root.icon.Contains(","))
                    {
                        root.icon = root.icon.Split(',')[0].Trim();
                    }
                    Icon ico = Icon.ExtractAssociatedIcon($@"{root.icon}");
                    Bitmap bitmap = ico.ToBitmap();
                    string tempfile = System.IO.Path.GetTempFileName();
                    Debug.WriteLine(tempfile);
                    Debug.WriteLine(System.IO.Path.GetTempPath());
                    bitmap.Save(tempfile, System.Drawing.Imaging.ImageFormat.Png);
                    Uri imageurl = new Uri(tempfile);
                    tmpList.Add(imageurl.ToString());
                    lists.source = imageurl.ToString();

                }
                catch
                {
                    lists.source = $@"{AppDomain.CurrentDomain.BaseDirectory}/Assets/icon.png";
                }

                lists.path = root.path;
                listslist.Add(lists);
                //ImageSource imagesrc = new BitmapImage(imageurl);
            }
            listView.ItemsSource = listslist;
            AllApplication.Text = $"{listslist.ToArray().Length}個のアプリが見つかりました";
            #endregion


        }

        private void ToggleSwitch_Toggled(object sender, Microsoft.UI.Xaml.RoutedEventArgs e)
        {
            ToggleSwitch toggleSwitch = sender as ToggleSwitch;
            StackPanel stackpanel = (StackPanel)toggleSwitch.Parent;
            UIElementCollection uIElements = stackpanel.Children;
            TextBlock textBlock = (TextBlock)uIElements.First();

            Border border = (Border)((Grid)stackpanel.Parent).Parent;

            string tag = "";
            if (border.Tag != null)
            {
                tag = border.Tag.ToString();
            }

            Debug.WriteLine(tag);

            if (toggleSwitch != null)
            {
                if (toggleSwitch.IsOn == true)
                {
                    textBlock.Text = "表示";

                    if (tag.StartsWith("HKLM"))
                    {
                        Debug.WriteLine(tag.Substring(5));
                        Microsoft.Win32.RegistryKey key;
                        key = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(tag.Substring(5));
                        if (key.GetValue("SystemComponent") != null)
                        {
                            key.DeleteValue("SystemComponent");
                        }
                        key.Close();
                    }
                    else if (tag.StartsWith("HKCU"))
                    {
                        Debug.WriteLine(tag.Substring(5));
                        Microsoft.Win32.RegistryKey key;
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(tag.Substring(5));
                        if (key.GetValue("SystemComponent") != null)
                        {
                            key.DeleteValue("SystemComponent");
                        }
                        key.Close();
                    }
                }
                else
                {
                    textBlock.Text = "非表示";
                    if (tag.StartsWith("HKLM"))
                    {
                        Debug.WriteLine(tag.Substring(5));
                        Microsoft.Win32.RegistryKey key;
                        key = Microsoft.Win32.Registry.LocalMachine.CreateSubKey(tag.Substring(5));
                        Int32 i = 1;
                        key.SetValue("SystemComponent", i);
                        key.Close();
                    }
                    else if (tag.StartsWith("HKCU"))
                    {
                        Debug.WriteLine(tag.Substring(5));
                        Microsoft.Win32.RegistryKey key;
                        key = Microsoft.Win32.Registry.CurrentUser.CreateSubKey(tag.Substring(5));
                        Int32 i = 1;
                        key.SetValue("SystemComponent", i);
                        key.Close();
                    }
                }
            }


        }

        private void QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            Debug.WriteLine(sender.Text);
            if (sender.Text == "")
            {
                listView.ItemsSource = listslist;
                AllApplication.Text = $"{listslist.ToArray().Length}個のアプリが見つかりました";
            }
            else
            {
                List<Lists> QueryList = listslist.FindAll((a) => a.ApplicationName.ToLower().Contains(sender.Text.ToLower()));
                listView.ItemsSource = QueryList;
                AllApplication.Text = $"{QueryList.ToArray().Length}個のアプリが見つかりました";
            }
        }

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs args)
        {
            TextBlock textBlock = (TextBlock)((ComboBox)sender).SelectedItem;
            Debug.WriteLine(textBlock.Name);
            if (textBlock.Name == "visible")
            {
                List<Lists> QueryList = listslist.FindAll((a) => a.isHide==true);
                listView.ItemsSource = QueryList;
                AllApplication.Text = $"{QueryList.ToArray().Length}個のアプリが見つかりました";
            }
            else if (textBlock.Name == "invisible")
            {
                List<Lists> QueryList = listslist.FindAll((a) => a.isHide==false);
                listView.ItemsSource = QueryList;
                AllApplication.Text = $"{QueryList.ToArray().Length}個のアプリが見つかりました";
            }
            else if(textBlock.Name == "none")
            {
                if (listslist != null && listView!=null)
                {
                    listView.ItemsSource = listslist;
                    AllApplication.Text = $"{listslist.ToArray().Length}個のアプリが見つかりました";
                }
            }
            else
            {

            }
        }
    }
}

namespace JSON
{
    [JsonObject("Root")]
    public class Root
    {

        [JsonProperty("path")]
        public string path { get; set; }

        [JsonProperty("icon")]
        public string icon { get; set; }

        [JsonProperty("name")]
        public string name { get; set; }

        [JsonProperty("version")]
        public string version { get; set; }

        [JsonProperty("publisher")]
        public string publisher { get; set; }

        [JsonProperty("InstallDate")]
        public string InstallDate { get; set; }

        [JsonProperty("isHide")]
        public bool isHide { get; set; }
    }

}



