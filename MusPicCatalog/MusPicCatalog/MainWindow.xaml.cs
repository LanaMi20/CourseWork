using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json;
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

namespace MusPicCatalog
{
    public partial class MainWindow : Window
    {
        List<MusPic> pics = new List<MusPic>();
        List<MusPic> temp_pics = new List<MusPic>();

        static string fileName = "MusPicCatalog.json";

        public MainWindow()
        {
            InitializeComponent();

            categories.Items.Add("all");

            if (File.Exists(fileName))
            {
                List<MusPic> readed_memes = JsonSerializer.Deserialize<List<MusPic>>(File.ReadAllText(fileName));

                foreach (MusPic mem in readed_memes)
                {
                    pics.Add(mem);

                    picturesL.Items.Add(mem.Name);

                    if (!(categories.Items.Contains(mem.Category)))
                        categories.Items.Add(mem.Category);

                }
            }
        }
        
        private void poisk_po_imeni_Click(object sender, RoutedEventArgs e)
        {
            picturesL.Items.Clear();
            temp_pics.Clear();
            foreach (MusPic pic in pics)
            {
                if (pic.Name.ToLower().Contains(poisk_name.Text.ToLower()))
                {
                    picturesL.Items.Add(pic.Name);
                    temp_pics.Add(pic);
                }
            }
        }

        private void delete_pic_Click(object sender, RoutedEventArgs e)
        {
            if (picturesL.SelectedIndex != -1 && picturesL.Items.Count == pics.Count)
            {
                pics.Remove(pics[picturesL.SelectedIndex]);
                picturesL.Items.Clear();

                foreach (MusPic mem in pics)
                    picturesL.Items.Add(mem.Name);
            }
            else
                MessageBox.Show("Select category all");
        }

        private void save_pics_Click(object sender, RoutedEventArgs e)
        {
            string jsonString = JsonSerializer.Serialize(pics);
            File.WriteAllText(fileName, jsonString);
        }

        private void add_tag_Click(object sender, RoutedEventArgs e)
        {
            if (picturesL.SelectedIndex != -1 && picturesL.Items.Count == pics.Count && addT.Text.Length > 0)
            {
                pics[picturesL.SelectedIndex].add_tag(addT.Text);
            }
            else
                MessageBox.Show("Select category all or fill in the tag field");

        }

        private void teg_poisk_Click(object sender, RoutedEventArgs e)
        {
            picturesL.Items.Clear();
            temp_pics.Clear();
            foreach (MusPic mem in pics)
            {
                foreach (string tag in mem.Tags)
                {
                    if (tag.ToLower().Equals(naiti_tag.Text.ToLower()))
                    {
                        picturesL.Items.Add(mem.Name);
                        temp_pics.Add(mem);
                    }
                }
            }
        }

        private void URL_Click(object sender, RoutedEventArgs e)
        {
            add_URL add_url_pic_wnd = new add_URL();

            if (add_url_pic_wnd.ShowDialog() == true)
            {
                WebClient client = new WebClient();

                string imageUrl = add_url_pic_wnd.url.Text;

                byte[] imageArray = client.DownloadData(imageUrl);

                string base64ImageRepresentation = Convert.ToBase64String(imageArray);

                MusPic mem = new MusPic(add_url_pic_wnd.name.Text, base64ImageRepresentation, add_url_pic_wnd.categr.Text);

                pics.Add(mem);
                picturesL.Items.Add(mem.Name);
                 
                if (!(categories.Items.Contains(mem.Category))) 
                    categories.Items.Add(mem.Category);
            }
        }

        private void Downland_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (!(bool)dlg.ShowDialog())
                return;

            Uri fileUri = new Uri(dlg.FileName);

            add_comp add_pic_wnd = new add_comp();

            if (add_pic_wnd.ShowDialog() == true)
            {
                byte[] imageArray = System.IO.File.ReadAllBytes(dlg.FileName);
                string base64ImageRepresentation = Convert.ToBase64String(imageArray);

                MusPic mem = new MusPic(add_pic_wnd.name.Text, base64ImageRepresentation, add_pic_wnd.categr.Text);

                pics.Add(mem);
                picturesL.Items.Add(mem.Name);

                if (!(categories.Items.Contains(mem.Category)))
                    categories.Items.Add(mem.Category);
            }
        }
        static ImageSource ByteToImage(byte[] imageData)
        {
            var bitmap = new BitmapImage();
            MemoryStream ms = new MemoryStream(imageData);
            bitmap.BeginInit();
            bitmap.StreamSource = ms;
            bitmap.EndInit();

            return (ImageSource)bitmap;
        }

        private void picturesL_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if ((categories.SelectedIndex == -1 || categories.SelectedIndex == 0) && (poisk_name.Text.Length == 0 && addT.Text.Length == 0) || temp_pics.Count == 0)
            {
                if (picturesL.SelectedIndex != -1)
                    PicImg.Source = ByteToImage(Convert.FromBase64String(pics[picturesL.SelectedIndex].Img));
            }
            else
                if (picturesL.SelectedIndex != -1)
                    PicImg.Source = ByteToImage(Convert.FromBase64String(temp_pics[picturesL.SelectedIndex].Img));
        }
    

        private void categories_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (categories.SelectedIndex != -1)
            {
                picturesL.Items.Clear();
                if (categories.SelectedItem.ToString().Equals("all"))
                {
                    foreach (MusPic mem in pics)
                        picturesL.Items.Add(mem.Name);
                    return;
                }

                temp_pics.Clear();

                foreach (MusPic mem in pics)
                {
                    if (mem.Category == categories.SelectedItem.ToString())
                    {
                        picturesL.Items.Add(mem.Name);
                        temp_pics.Add(mem);
                    }
                }
            }
        }
    }
}
