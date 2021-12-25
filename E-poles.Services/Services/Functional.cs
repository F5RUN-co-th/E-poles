using E_poles.Dal;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
//using SendGrid;
//using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace E_poles.Services
{
    public class Functional : IFunctional
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<Role> _roleManager;
        private readonly ApplicationDbContext _context;
        private readonly SignInManager<User> _signInManager;
        private readonly IRoles _roles;
        private readonly SuperAdminDefaultOptions _superAdminDefaultOptions;

        public Functional(UserManager<User> userManager,
           RoleManager<Role> roleManager,
           ApplicationDbContext context,
           SignInManager<User> signInManager,
           IRoles roles,
           IOptions<SuperAdminDefaultOptions> superAdminDefaultOptions)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _context = context;
            _signInManager = signInManager;
            _roles = roles;
            _superAdminDefaultOptions = superAdminDefaultOptions.Value;
        }

        public async Task InitAppData()
        {
            try
            {
                if (!_context.Poles.Any())
                {

                    List<Poles> poles = new List<Poles>() {
                    new Poles{ Name = "number 1",Latitude= 14.106743,Longitude=100.396078},
                    new Poles{ Name = "number 2",Latitude= 13.723910,Longitude=102.322485},
                    new Poles{ Name = "number 3",Latitude= 12.862147,Longitude=99.895083},
                };
                    await _context.Poles.AddRangeAsync(poles);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task CreateDefaultSuperAdmin()
        {
            try
            {
                await _roles.GenerateRolesFromPagesAsync();

                User superAdmin = new User
                {
                    UserName = _superAdminDefaultOptions.UserName,
                    Email = _superAdminDefaultOptions.Email,
                    EmailConfirmed = true
                };

                var result = await _userManager.CreateAsync(superAdmin, _superAdminDefaultOptions.Password);

                if (result.Succeeded)
                {
                    //add to user profile
                    UserProfile profile = new UserProfile();
                    profile.FirstName = "Super";
                    profile.LastName = "Admin";
                    profile.Email = superAdmin.Email;
                    profile.UserId = superAdmin.Id;
                    await _context.UserProfile.AddAsync(profile);
                    await _context.SaveChangesAsync();

                    await _roles.AddToRoles(superAdmin.Id);
                }
            }
            catch (Exception)
            {

                throw;
            }
        }

        //public async Task SendEmailBySendGridAsync(string apiKey,
        //    string fromEmail,
        //    string fromFullName,
        //    string subject,
        //    string message,
        //    string email)
        //{
        //    var client = new SendGridClient(apiKey);
        //    var msg = new SendGridMessage()
        //    {
        //        From = new EmailAddress(fromEmail, fromFullName),
        //        Subject = subject,
        //        PlainTextContent = message,
        //        HtmlContent = message
        //    };
        //    msg.AddTo(new EmailAddress(email, email));
        //    await client.SendEmailAsync(msg);

        //}

        //public async Task SendEmailByGmailAsync(string fromEmail,
        //    string fromFullName,
        //    string subject,
        //    string messageBody,
        //    string toEmail,
        //    string toFullName,
        //    string smtpUser,
        //    string smtpPassword,
        //    string smtpHost,
        //    int smtpPort,
        //    bool smtpSSL)
        //{
        //    var body = messageBody;
        //    var message = new MailMessage();
        //    message.To.Add(new MailAddress(toEmail, toFullName));
        //    message.From = new MailAddress(fromEmail, fromFullName);
        //    message.Subject = subject;
        //    message.Body = body;
        //    message.IsBodyHtml = true;

        //    using (var smtp = new SmtpClient())
        //    {
        //        var credential = new NetworkCredential
        //        {
        //            UserName = smtpUser,
        //            Password = smtpPassword
        //        };
        //        smtp.Credentials = credential;
        //        smtp.Host = smtpHost;
        //        smtp.Port = smtpPort;
        //        smtp.EnableSsl = smtpSSL;
        //        await smtp.SendMailAsync(message);

        //    }

        //}

        //public async Task<string> UploadFile(List<IFormFile> files, IHostingEnvironment env, string uploadFolder)
        //{
        //    var result = "";

        //    var webRoot = env.WebRootPath;
        //    var uploads = System.IO.Path.Combine(webRoot, uploadFolder);
        //    var extension = "";
        //    var filePath = "";
        //    var fileName = "";


        //    foreach (var formFile in files)
        //    {
        //        if (formFile.Length > 0)
        //        {
        //            extension = System.IO.Path.GetExtension(formFile.FileName);
        //            fileName = Guid.NewGuid().ToString() + extension;
        //            filePath = System.IO.Path.Combine(uploads, fileName);

        //            using (var stream = new FileStream(filePath, FileMode.Create))
        //            {
        //                await formFile.CopyToAsync(stream);
        //            }

        //            result = fileName;

        //        }
        //    }

        //    return result;
        //}

    }
}
