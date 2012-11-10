using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Xml.Serialization;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// Die Elementvorlage "Leere Seite" ist unter http://go.microsoft.com/fwlink/?LinkId=234238 dokumentiert.

namespace NotesApp
{
    /// <summary>
    /// Eine leere Seite, die eigenständig verwendet werden kann oder auf die innerhalb eines Rahmens navigiert werden kann.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private ObservableCollection<String> itemsList = null;

        public MainPage()
        {
            this.InitializeComponent();

            itemsList = LoadList("notes", false);

            this.notesList.ItemsSource = itemsList;
        }

        /// <summary>
        /// Wird aufgerufen, wenn diese Seite in einem Rahmen angezeigt werden soll.
        /// </summary>
        /// <param name="e">Ereignisdaten, die beschreiben, wie diese Seite erreicht wurde. Die
        /// Parametereigenschaft wird normalerweise zum Konfigurieren der Seite verwendet.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void ListView_SelectionChanged_1(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            itemsList.Add(this.addItemTextBox.Text);
            SaveList("notes", itemsList, false);
        }

        public static ObservableCollection<String> LoadList(string name, bool roaming = true)
        {
            String serialized = null;

            if (roaming && ApplicationData.Current.RoamingSettings.Values.ContainsKey(name))
            {
                serialized = ApplicationData.Current.RoamingSettings.Values[name].ToString();
            }

            if (ApplicationData.Current.LocalSettings.Values.ContainsKey(name))
            {
                serialized = ApplicationData.Current.LocalSettings.Values[name].ToString();
            }

            if (serialized == null)
            {
                return new ObservableCollection<String>();
            }
            List<String> list;

            var _Bytes = Encoding.Unicode.GetBytes(serialized);
            using (MemoryStream _Stream = new MemoryStream(_Bytes))
            {
                var _Serializer = new DataContractJsonSerializer(typeof(List<String>));
                list = (List<String>)_Serializer.ReadObject(_Stream);
            }
            return new ObservableCollection<String>(list);
        }

        public static void SaveList(string name, ObservableCollection<String> list, bool roaming = true)
        {
            String serialized;

            using (MemoryStream _Stream = new MemoryStream())
            {
                var _Serializer = new DataContractJsonSerializer(list.ToList().GetType());
                _Serializer.WriteObject(_Stream, list.ToList());
                _Stream.Position = 0;
                using (StreamReader _Reader = new StreamReader(_Stream))
                { serialized = _Reader.ReadToEnd(); }
            }

            if (roaming)
                ApplicationData.Current.RoamingSettings.Values[name] = serialized;
            ApplicationData.Current.LocalSettings.Values[name] = serialized;
        }
    }
}
