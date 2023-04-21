using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SneakerOnlineShop.Models;
using System.Net.Mail;
using System.Text;

namespace SneakerOnlineShop.Pages.Account
{
    public class ForgotPasswordModel : PageModel
    {
        private readonly SWP391_DBContext _dbContext;
        public ForgotPasswordModel(SWP391_DBContext dBContext)
        {
            this._dbContext = dBContext;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(String email)
        {
            //Find in account with email
            var acc = _dbContext.Accounts.Where(acc => acc.Email.Equals(email)).FirstOrDefault();
            
            if(acc != null)
            {
                String newPass = RandomPassword();
                acc.Password = newPass;
                await _dbContext.SaveChangesAsync();
                MailMessage mail = new MailMessage();
                mail.To.Add(email.ToString().Trim());
                mail.From = new MailAddress("acc2hunglm@gmail.com");
                mail.Subject = "Password recovery";
                mail.Body = $@"<p>Hi,<br/> This is Sneaker Online Shop. Your new password is: {newPass}<br/>Have a good day!</p><br/><p>To change password, GO TO this link: https://localhost:7139/account/changepassword</p>";
                mail.IsBodyHtml = true;

                SmtpClient smtp = new SmtpClient();
                smtp.Port = 587;
                smtp.EnableSsl = true;
                smtp.UseDefaultCredentials = false;
                smtp.Host = "smtp.gmail.com";
                smtp.Credentials = new System.Net.NetworkCredential("acc2hunglm@gmail.com", "lmvahgbghhyxeqzb");
                smtp.Send(mail);
            }

            //Info to user
            ViewData["msg"] = "We have send password. Thankyou so much";
            return Page();
        }

        // Generate a random string with a given size and case.
        // If second parameter is true, the return string is lowercase
        public string RandomString(int size, bool lowerCase)
        {
            StringBuilder builder = new StringBuilder();
            Random random = new Random();
            char ch;
            for (int i = 0; i < size; i++)
            {
                ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                builder.Append(ch);
            }
            if (lowerCase)
                return builder.ToString().ToLower();
            return builder.ToString();
        }
        private static string RandomInputChars(int length, String validCharss)
        {
            // Create a string of characters, numbers, and special characters that are allowed in the password
            string vChars = validCharss;
            Random random = new Random();
            // Select one random character at a time from the string
            // and create an array of chars
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = vChars[random.Next(0, vChars.Length)];
            }
            return new string(chars);
        }
        // Generate a random number between two numbers
        public int RandomNumber(int min, int max)
        {
            Random random = new Random();
            return random.Next(min, max);
        }
        // Generate a random password of a given length (optional)
        public string RandomPassword()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(RandomString(1, true));
            builder.Append(RandomNumber(1, 9));
            builder.Append(RandomInputChars(1, "!@#$%^&*?_-"));
            builder.Append(RandomInputChars(7, "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*?_-"));
            return builder.ToString();
            
        }
    }
}
