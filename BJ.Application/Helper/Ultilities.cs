using BJ.Application.Email;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace BJ.Application.Helper
{
    public static class Utilities
    {
        public static string StripHTML(string input)
        {
            try
            {
                if (!string.IsNullOrEmpty(input))
                {
                    return Regex.Replace(input, "<.*?>", String.Empty);
                }
            }
            catch
            {
                return null;
            }
            return null;
        }
        public static bool IsValidEmail(string email)
        {
            if (email.Trim().EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }


        public static void CreateIfMissing(string path)
        {
            bool folderExists = Directory.Exists(path);
            if (!folderExists)
                Directory.CreateDirectory(path);
        }
        public static string ToTitleCase(string str)
        {
            string result = str;
            if (!string.IsNullOrEmpty(str))
            {
                var words = str.Split(' ');
                for (int index = 0; index < words.Length; index++)
                {
                    var s = words[index];
                    if (s.Length > 0)
                    {
                        words[index] = s[0].ToString().ToUpper() + s.Substring(1);
                    }
                }
                result = string.Join(" ", words);
            }
            return result;
        }
        public static bool IsInteger(string str)
        {
            Regex regex = new Regex(@"^[0-9]+$");

            try
            {
                if (String.IsNullOrWhiteSpace(str))
                {
                    return false;
                }
                if (!regex.IsMatch(str))
                {
                    return false;
                }

                return true;

            }
            catch
            {

            }
            return false;

        }
        public static string GetRandomKey(int length = 5)
        {
            //chuỗi mẫu (pattern)
            string pattern = @"0123456789zxcvbnmasdfghjklqwertyuiop[]{}:~!@#$%^&*()+";
            Random rd = new Random();
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < length; i++)
            {
                sb.Append(pattern[rd.Next(0, pattern.Length)]);
            }

            return sb.ToString();
        }
        public static string SEOUrl(string url)
        {
            url = url.ToLower();
            url = Regex.Replace(url, @"[áàạảãâấầậẩẫăắằặẳẵ]", "a");
            url = Regex.Replace(url, @"[éèẹẻẽêếềệểễ]", "e");
            url = Regex.Replace(url, @"[óòọỏõôốồộổỗơớờợởỡ]", "o");
            url = Regex.Replace(url, @"[íìịỉĩ]", "i");
            url = Regex.Replace(url, @"[ýỳỵỉỹ]", "y");
            url = Regex.Replace(url, @"[úùụủũưứừựửữ]", "u");
            url = Regex.Replace(url, @"[đ]", "d");

            //2. Chỉ cho phép nhận:[0-9a-z-\s]
            url = Regex.Replace(url.Trim(), @"[^0-9a-z-\s]", "").Trim();
            //xử lý nhiều hơn 1 khoảng trắng --> 1 kt
            url = Regex.Replace(url.Trim(), @"\s+", "-");
            //thay khoảng trắng bằng -
            url = Regex.Replace(url, @"\s", "-");
            while (true)
            {
                if (url.IndexOf("--") != -1)
                {
                    url = url.Replace("--", "-");
                }
                else
                {
                    break;
                }
            }
            return url;
        }

        public static string GenerateStringDateTime()
        {
            var day = DateTime.Now.Day;
            var month = DateTime.Now.Month;
            var year = DateTime.Now.Year;
            var hour = DateTime.Now.Hour;
            var minute = DateTime.Now.Minute;
            var second = DateTime.Now.Second;
            var ms = DateTime.Now.Millisecond;
            var date = day + month + year + "T" + hour + minute + second + ms;

            return date;
        }


        public static async Task<string> UploadFile(Microsoft.AspNetCore.Http.IFormFile file, string sDirectory, string newname = null)
        {
            try
            {

                if (newname == null) newname = file.FileName;
                string path = Path.Combine(Directory.GetCurrentDirectory(), sDirectory);
                CreateIfMissing(path);
                string pathFile = Path.Combine(Directory.GetCurrentDirectory(), sDirectory, newname);
                var supportedTypes = new[] { "jpg", "jpeg", "png", "gif", "webp", "docx", "doc", "xlsx", "pdf", "xls", "txt", "csv", "mp4" };
                var fileExt = System.IO.Path.GetExtension(file.FileName).Substring(1);
                if (!supportedTypes.Contains(fileExt.ToLower())) /// Khác các file định nghĩa
                {
                    return null;
                }
                else
                {
                    using (var stream = new FileStream(pathFile, FileMode.Create))
                    {
                        await file.CopyToAsync(stream);
                    }
                    return newname;
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }


        public static string DecryptString(string cipherText, string keyString)
        {
            var fullCipher = Convert.FromBase64String(cipherText);

            var iv = new byte[16];
            var cipher = new byte[16];

            Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
            Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, iv.Length);
            var key = Encoding.UTF8.GetBytes(keyString);

            using (var aesAlg = Aes.Create())
            {
                using (var decryptor = aesAlg.CreateDecryptor(key, iv))
                {
                    string result;
                    using (var msDecrypt = new MemoryStream(cipher))
                    {
                        using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                        {
                            using (var srDecrypt = new StreamReader(csDecrypt))
                            {
                                result = srDecrypt.ReadToEnd();
                            }
                        }
                    }

                    return result;
                }
            }
        }
        public static Message MailFeedBack(string reason, string fullname, string email, string emailTo, string phone, string vibe_member, string store_name, string desc, DateTime dateSend)
        {
            var body = "<div class=\"\">" +
                            "<div class=\"aHl\">" +
                            "</div><div id=\":pl\" tabindex=\"-1\"></div><div id=\":pa\" class=\"ii gt\" jslog=\"20277; u014N:xr6bB; 1:WyIjdGhyZWFkLWY6MTc1OTk0MzE2OTU1MDk1NjgzOCIsbnVsbCxudWxsLG51bGwsbnVsbCxudWxsLG51bGwsbnVsbCxudWxsLG51bGwsbnVsbCxudWxsLG51bGwsW11d; 4:WyIjbXNnLWY6MTc1OTk0MzE2OTU1MDk1NjgzOCIsbnVsbCxbXV0.\"><div id=\":p9\" class=\"a3s aiL \"><u></u>\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n\r\n<div style=\"background-color:#e5e5e5;font-family:sans-serif;font-size:12px;line-height:1.4;margin:0;padding:0\">\r\n" +
                            "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse:separate;width:100%;padding:0;background-color:#ffffff\">\r\n<tbody>\r\n<tr>\r\n<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top\">&nbsp;</td>\r\n<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top;display:block;margin:0 auto;max-width:800px;padding:0;width:800px\">\r\n<div style=\"box-sizing:border-box;display:block;Margin:0;max-width:800px;padding:0\">\r\n\r\n\r\n" +
                            "<div style=\"display:inline-block;clear:both;text-align:center;width:100%;background-size:cover;background:rgba(233,234,247,0.5);\">\r\n" +
                            "<img src=\"https://imgur.com/pFFMz6l.png\" alt=\"VXH.Booking\" style=\"float:left;padding:5% 0 4% 5%;max-width:18%;height:auto;min-height:12px\" class=\"CToWUd\" data-bit=\"iit\">\r\n" +
                            "</div>\r\n<table style=\"border-collapse:separate;width:100%;background:#ffffff;border-radius:3px\">\r\n\r\n\r\n<tbody><tr>\r\n" +
                            "<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top;box-sizing:border-box;padding:5%;padding-bottom:0\">\r\n" +
                            "<table border=\"0\" cellpadding=\"0\" cellspacing=\"0\" style=\"border-collapse:separate;width:100%\">\r\n<tbody><tr>\r\n" +
                            "<td style=\"font-family:sans-serif;font-size:12px;vertical-align:top\">\r\n" +
                            "<h1 style=\"font-weight:bold;font-size:18px;line-height:24px;color: #70b046;margin-bottom:24px;margin-top:0\">" +
                            "Xin chào,</h1>\r\n" +
                            "<div style=\"background:rgba(233,234,247,0.5);margin-bottom:16px;text-align:left;padding:24px 30px 24px\">\r\n" +
                            "<p style=\"font-weight:500;margin-top:0;margin-bottom:16px;font-size:14px;line-height:20px;color:#70b046\">\r\n\t\t\t\t\t\t\t\t\t\t\t<b>\r\n\t\t\t\t\t\t\t\t\t\t\t<span>" +
                            "Bạn có một phản hồi từ: </span>\r\n" +
                            "\r\n\t\t\t\t\t\t\t\t\t\t\t\r\n\r\n\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t\t</b>" +
                            "<b style=\"font-size:14px;color: #70b046\"><a style=\"font-size:14px;color: #70b046\" href=mailto:" + email + ">" + email + "</a></b></p><br>" +
                            "<b style=\"font-size:14px;color: #70b046\"><a style=\"font-size:14px;color: #70b046\">Người phản hồi: </a></b>" + "<span style=\"color: #70b046;font-size:14px\"> " + fullname + "</span><br>\r\n\t\t\t\t\t\t\t\t\t" +
                            "<b style=\"font-size:14px;color: #70b046\"><a style=\"font-size:14px;color: #70b046\">Email: </a></b>" + "<span style=\"color: #70b046;font-size:14px\"> " + email + "</span><br>\r\n\t\t\t\t\t\t\t\t\t" +
                            "<b style=\"font-size:14px;color: #70b046\"><a style=\"font-size:14px;color: #70b046\">Mã thành viên: </a></b>" + "<span style=\"color: #70b046;font-size:14px\"> " + vibe_member + "</span><br>\r\n\t\t\t\t\t\t\t\t\t" +
                            "<b style=\"font-size:14px;color: #70b046\"><a style=\"font-size:14px;color: #70b046\">Số điện thoại: </a></b>" + "<span style=\"color: #70b046;font-size:14px\"> " + phone + "</span><br>\r\n\t\t\t\t\t\t\t\t\t" +


                            "<b style=\"font-size:14px;color: #70b046\"><a style=\"font-size:14px;color: #70b046\">Tiêu đề: </a></b>" + "<span style=\"color: #70b046;font-size:14px\"> " + reason + "</span><br>\r\n\t\t\t\t\t\t\t\t\t" +

                            "<b style=\"font-size:14px;color: #70b046\"<a style=\"font-size:14px;color: #70b046\">Nội dung: </a></b><br>" + "<span style=\"color: #70b046;font-size:14px\"> " + desc + "</span><br>\r\n\t\t\t\t\t\t\t\t\t" +
                            "<b style=\"font-size:14px;color: #70b046\"<a style=\"font-size:14px;color: #70b046\">Cửa hàng: </a></b><br>" + "<span style=\"color: #70b046;font-size:14px\"> " + store_name + "</span><br>\r\n\t\t\t\t\t\t\t\t\t" +

                            "<div>\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t" +
                            "<p style=\"font-weight:500;margin-top:0;margin-bottom:4px;font-size:14px;line-height:18px;color:red\">" +
                            "</p>\r\n\t\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t" +
                            "<div> \r\n\r\n\t\t\t\t\t\t\t\t\t\t<p style=\"font-weight:bold;margin-top:0;margin-bottom:4px;font-size:16px;text-decoration:underline;line-height:18px;color:#71787e\">" +
                            "Thời gian gửi phản hồi:</p>\r\n" +
                            "<p style=\"font-weight:bold;font-size:14px\">Thời gian gửi: <b style=\"color:red\">" + dateSend + "</b></p>\r\n" +

                            "\r\n\t\t\t\t\t\t\t\t\t\t\t\t\r\n\t\t\t\t\t\t\t\t\t\t\t\t<br>\r\n\t\t\t\t\t\t\t</div>\r\n\t\t\t\t\t\t\t\t\t\t\t\t\t\t\r\n</div>\r\n" +
                            "</td>\r\n</tr>\r\n</tbody></table>\r\n</td>\r\n</tr>\r\n" +
                            "<p style=\"font-weight:600;font-size:13px;line-height:18px;margin-top:0;margin-bottom:4px\">GIỚI THIỆU VỀ BOOST JUICE</p>\r\n<p style=\"color:#71787e;font-weight:normal;font-size:12px;line-height:18px;margin-top:0;margin-bottom:10px\">Về Thương hiệu Smoothie và Nước ép trái cây được ưa chuộng nhất tại Úc.</p>\r\n" +

                            "<div class=\"yj6qo\"></div><div class=\"adL\">\r\n</div></div><div class=\"adL\">\r\n\r\n\r\n</div></div></div><div id=\":pp\" class=\"ii gt\" style=\"display:none\"><div id=\":pq\" class=\"a3s aiL \"></div></div><div class=\"hi\"></div>" +
            "</div>";

            var message = new Message(new string[] { $"{emailTo}" }, "PHẢN HỒI BOOSTJUICE", $"{body}");

            return message;

        }
    }

}
