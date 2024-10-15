using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Numerics;
using System.Text.RegularExpressions;


namespace CMS_API_Core.Validations
{
    public class ModelValidations : Dictionary<string, string>
    {

        Dictionary<string, string> errors = new();


        public ModelValidations IsNullOrEmpty(string value, string key, string errorMassage)
        {

            if (string.IsNullOrEmpty(value))
            {
                errors.TryAdd(key, errorMassage);

            }
            return this;

        }
        public ModelValidations IsTrueOrFalse(bool? value, string key, string errorMassage)
        {
            if (value != null)
            {


                if (value != true && value != false)
                {
                    errors.TryAdd(key, errorMassage);

                }
            }
            return this;

        }

        public ModelValidations IsString(string value, string key, string errorMassage)
        {
            if (value != null)
            {

                if (!errors.ContainsKey(key))
                {
                    if (!value.All(char.IsLetter))
                    {
                        errors.TryAdd(key, errorMassage);
                    }

                    //var r = new Regex(@"^[a-z]+[A-Z]?$");
                    //if (!r.IsMatch(value))
                    //{
                    //    errors.TryAdd(key, errorMassage);
                    //}
                }
            }
            return this;

        }
        public ModelValidations IsNotString(string value, string key, string errorMassage)
        {
            if (value != null)
            {

                if (!errors.ContainsKey(key))
                {
                    foreach (var ch in value)
                    {
                        if (!char.IsDigit(ch))
                        {
                            errors.TryAdd(key, errorMassage + " must contain only numbers.");
                            break;
                        }
                    }
                }
            }
            return this;

        }
        public ModelValidations IsPhoneValid(string phone, string key, string errorMassage)
        {
            if (phone != null)
            {

                if (!errors.ContainsKey(key))
                    if (phone.Length != 11)
                    {
                        errors.TryAdd(key, errorMassage + " it must be 11 Numbers");
                    }
                    else if (!phone.StartsWith("01"))
                    {
                        errors.TryAdd(key, errorMassage + "it must start with (01)");
                    }
                    else
                    {
                        IsNotString(phone, key, errorMassage);
                    }
            }

            //var r = new Regex(@"^[0][1][0-9]{9}$");
            //if (!r.IsMatch(phone))
            //{
            //    errors.Add(key, errorMassage);
            //}
            return this;

        }

        public ModelValidations IsDateValid(DateTime olddate, string key, string errorMassage)
        {
            if (olddate != null)
            {

                string stringDate = olddate.ToString();

                DateTime NewDate;

                if (!DateTime.TryParse(stringDate, out NewDate))
                {
                    errors.TryAdd(key, errorMassage);

                }
            }
            return this;



        }

        public ModelValidations IsDateValid(string olddate, string key, string errorMassage)
        {
            if (olddate != null)
            {
                DateTime NewDate;

                if (DateTime.TryParse(olddate, out NewDate))
                {
                    errors.TryAdd(key, errorMassage);

                }
            }

            return this;


        }

        public ModelValidations IsContainForbiddenValue(string content, string[] ForbiddenList, string key, string errorMassage)
        {
            if (content != null)
            {
                foreach (var item in ForbiddenList)
                {
                    if (content.Contains(item))
                    {
                        errors.TryAdd(key, errorMassage);
                    }
                }
            }
            return this;

        }


        public ModelValidations IsEmailValid(string email, string key, string errorMassage)
        {
            if (email != null)
            {
                if (!errors.ContainsKey(key))
                    if (email.Contains("@") && email.Contains("."))
                    {
                        try
                        {
                            var addr = new MailAddress(email);
                        }
                        catch
                        {
                            errors.TryAdd(key, errorMassage + "e.g test@test.com");
                        }
                    }
                    else errors.TryAdd(key, errorMassage + "It dosen`t contain (@) or(.)");
            }
            return this;

        }





        public ModelValidations IsPasswordValid(string password, string key, string errorMassage)
        {
            if (password != null)
            {

                if (!errors.ContainsKey(key))
                {

                    IsContainForbiddenValue(password, ["Test", ">", "<", "^", "(", ")", "  "], key, errorMassage);

                    if (!errors.ContainsKey(key) && (
                        password.Length > 300 ||
                        password.Length < 8 ||
                       !new Regex(@"^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$ %^&*-]).{8,}$").IsMatch(password)
                        ))
                    {
                        if (password.Length < 8)
                        {
                            errors.TryAdd(key, errorMassage + "Password must be at least 8 characters long.");
                        }
                        else if (password.Length > 300)
                        {
                            errors.TryAdd(key, errorMassage + "Password cannot exceed 300 characters.");
                        }
                        else if (!Regex.IsMatch(password, @"[A-Z]"))
                        {
                            errors.TryAdd(key, errorMassage + "Password must contain at least one uppercase letter. ");
                        }
                        else if (!Regex.IsMatch(password, @"[a-z]"))
                        {
                            errors.TryAdd(key, errorMassage + "Password must contain at least one lowercase letter. ");
                        }
                        else if (!Regex.IsMatch(password, @"[0-9]"))
                        {
                            errors.TryAdd(key, errorMassage + "Password must contain at least one digit. ");
                        }
                        else if (!Regex.IsMatch(password, @"[#?!@$%^&*-]"))
                        {
                            errors.TryAdd(key, errorMassage + "Password must contain at least one special character (e.g., #?!@$%^&*-). ");
                        }

                    }
                }
            }
            return this;
        }

        public ModelValidations IsUsernameValid(string username, string key, string errorMassage)
        {
            if (username != null)
            {

                if (!errors.ContainsKey(key))
                {
                    var r = new Regex(@"^[@,#]?[a-zA-Z][a-zA-Z0-9._]{1,13}[a-zA-Z0-9]$");
                    if (username.Length < 3)
                    {
                        errors.TryAdd(key, errorMassage + "Username must be at least 3 characters long. ");
                    }
                    else if (username.Length > 15)
                    {
                        errors.TryAdd(key, errorMassage + "Username cannot exceed 15 characters. ");
                    }


                    else if (!r.IsMatch(username))
                    {
                        if (username.Length > 0 && !char.IsLetter(username[0]))
                        {
                            errors.TryAdd(key, errorMassage + "Username must start with a letter. ");
                        }
                        else if (username.Length > 0 && (username[username.Length - 1] == '_' || username[username.Length - 1] == '.'))
                        {
                            errors.TryAdd(key, errorMassage + "Username cannot end with an underscore or period. ");
                        }
                        else if (username.Contains("__") || username.Contains(".."))
                        {
                            errors.TryAdd(key, errorMassage + "Username cannot contain consecutive underscores or periods. ");
                        }
                        else if (username.Contains("@") || username.Contains("#"))
                        {
                            errors.TryAdd(key, errorMassage + "Username can contain only letters, numbers, underscores, and periods. Special characters such as @ or # are not allowed in the middle. ");
                        }
                    }


                }
            }
            return this;

        }



        public ModelValidations IsExtintionValid(string content, string[] extintions, string key, string errorMassage)
        {
            if (content != null)
            {


                foreach (var ext in extintions)
                {
                    if (content.Contains(ext))
                    {
                        return this;
                    }
                }
                errors.TryAdd(key, errorMassage);
            }
            return this;

        }


        public Dictionary<string, string> GetValidations()
        {
            return errors;
        }

    }
}
