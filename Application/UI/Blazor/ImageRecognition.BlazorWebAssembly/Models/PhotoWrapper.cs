﻿using System;
using System.ComponentModel;
using ImageRecognition.API.Client;
using Newtonsoft.Json;

namespace ImageRecognition.BlazorWebAssembly.Models
{
    public class PhotoWrapper : INotifyPropertyChanged
    {
        private string _status;

        public PhotoWrapper(Photo photo)
        {
            Photo = photo;
            _status = Photo.ProcessingStatus.ToString();
        }

        public Photo Photo { get; set; }

        public string Status
        {
            get
            {
                if (Photo.ProcessingStatus == ProcessingStatus.Failed) return "Failed";
                return _status;
            }
            set
            {
                _status = value;
                OnPropertyChanged("Status");
            }
        }

        public void Update(MessageEvent evnt)
        {
            if (string.Equals(Photo.PhotoId, evnt.ResourceId, StringComparison.Ordinal))
            {
                if (evnt.CompleteEvent)
                {
                    var photo = JsonConvert.DeserializeObject<Photo>(evnt.Data);

                    var signedThumbnailUrl = Photo.Thumbnail.Url;
                    var signedFullSizeUrl = Photo.FullSize.Url;

                    if (photo != null) Photo = photo;

                    Photo.Thumbnail.Url = signedThumbnailUrl;
                    Photo.FullSize.Url = signedFullSizeUrl;
                    Photo.ProcessingStatus = ProcessingStatus.Succeeded;
                    Status = ProcessingStatus.Succeeded.ToString();
                }
                else
                {
                    Status = evnt.Message;
                }
            }
        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}