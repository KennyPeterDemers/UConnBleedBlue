using System.Xml;

namespace UConnBleedBlue.Models
{
    public delegate void Callback(string message);
    public class EmailRequest
    {
        Callback? _Callback;
        public string From { get; set; } = "";  // will come from form
        public string FromEmail { get; set; } = "";  // will come from form

        private string _tribute = "";
        public string Subject { get; set; } = "2024 Tribute To Andy";
        public string Tribute
        {
            get
            {
                return _tribute;
            }
            set
            {
                _tribute = value;
                if (_Callback != null)
                {
                    _Callback(_tribute);
                }
            }
        }

        public void SetCallback(Callback callback)
        {
            _Callback = callback;
        }
    }
}
