using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApp4
{
    public partial class PhotoAlbumForm : Form
    {
        private PhotoAlbum photoAlbum;
        private ListView listView;
        private Button addPhotoButton;
        private Button removePhotoButton;
        private Button sortByDateButton;
        public PhotoAlbumForm()
        {
            this.Text = "Управление фотографиями";
            this.Width = 600;
            this.Height = 400;
            CreateControls();
            photoAlbum = new PhotoAlbum(listView);
        }
        private void CreateControls()
        {
            listView = new ListView
            {
                Location = new System.Drawing.Point(10, 10),
                Size = new System.Drawing.Size(580, 350),
                View = View.Details,
                FullRowSelect = true
            };
            listView.Columns.Add("Путь", 300);
            listView.Columns.Add("Описание", 200);
            listView.Columns.Add("Дата съёмки", 100);
            addPhotoButton = new Button
            {
                Location = new System.Drawing.Point(10, 370),
                Text = "Добавить фото",
                Size = new System.Drawing.Size(100, 25)
            };
            addPhotoButton.Click += (sender, e) => photoAlbum.AddPhoto();
            removePhotoButton = new Button
            {
                Location = new System.Drawing.Point(120, 370),
                Text = "Удалить фото",
                Size = new System.Drawing.Size(100, 25)
            };
            removePhotoButton.Click += (sender, e) => photoAlbum.RemovePhoto();
            sortByDateButton = new Button
            {
                Location = new System.Drawing.Point(230, 370),
                Text = "Отсортировать по дате",
                Size = new System.Drawing.Size(120, 25)
            };
            sortByDateButton.Click += (sender, e) => photoAlbum.SortPhotosByDate();
            this.Controls.Add(listView);
            this.Controls.Add(addPhotoButton);
            this.Controls.Add(removePhotoButton);
            this.Controls.Add(sortByDateButton);
        }
    }
        public class Photo
    {
        public string Path { get; set; }
        public string Description { get; set; }
        public DateTime DateTaken { get; set; }
        public Photo(string path, string description, DateTime dateTaken)
        {
            Path = path;
            Description = description;
            DateTaken = dateTaken;
        }
        public override string ToString()
        {
            return $"{Path} - {Description} ({DateTaken.ToString("dd.MM.yyyy")})";
        }
    }
    public class PhotoAlbum
    {
        private List<Photo> photos = new List<Photo>();
        private ListView listView;
        public PhotoAlbum(ListView listView)
        {
            this.listView = listView;
            LoadPhotos();
        }
        private void LoadPhotos()
        {
            listView.Items.Clear();
            foreach (var photo in photos)
            {
                listView.Items.Add(new ListViewItem(new[] { photo.Path, photo.Description,
                photo.DateTaken.ToString("dd.MM.yyyy") }));
            }
        }
        public void AddPhoto()
        {
            using (var openFileDialog = new OpenFileDialog())
            {
                openFileDialog.InitialDirectory =
                Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                openFileDialog.Title = "Выберите фото";
                openFileDialog.Filter = "Изображения (*.jpg;*.png;*.jpeg)|*.jpg;*.png;*.jpeg";
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    var photoPath = openFileDialog.FileName;
                    var description = GetDescription();
                    var dateTaken = DateTime.ParseExact(description, "dd.MM.yyyy", null);
                    photos.Add(new Photo(photoPath, description, dateTaken));
                    LoadPhotos();
                    MessageBox.Show("Фото добавлено.");
                }
            }
        }
        public void RemovePhoto()
        {
            if (listView.SelectedItems.Count == 0)
            {
                MessageBox.Show("Сначала выберите фото для удаления.");
                return;
            }
            var photoPath = listView.SelectedItems[0].SubItems[0].Text;
            photos.RemoveAll(p => p.Path == photoPath);
            LoadPhotos();
            MessageBox.Show("Фото удалено.");
        }
        public void SortPhotosByDate()
        {
            var sortedPhotos = photos.OrderBy(p => p.DateTaken).ToList();
            photos = new List<Photo>(sortedPhotos);
            LoadPhotos();
            MessageBox.Show("Фото отсортированы по дате.");
        }
        private string GetDescription()
        {
            using (var descriptionForm = new DescriptionForm())
            {
                descriptionForm.ShowDialog();
                return "";
                // return descriptionForm.Description;
            }
        }
    }
}
