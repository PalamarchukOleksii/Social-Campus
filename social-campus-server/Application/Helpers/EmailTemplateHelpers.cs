namespace Application.Helpers;

public static class EmailTemplateHelpers
{
    public static string GetVerifyEmailHtml(string firstName, string verificationLink)
    {
        return $$"""
                 <!DOCTYPE html>
                 <html lang="en">
                 <head>
                     <meta charset="UTF-8" />
                     <meta name="viewport" content="width=device-width, initial-scale=1.0" />
                     <title>Verify Your Email</title>
                     <style>
                         @import url("https://fonts.googleapis.com/css2?family=Roboto:wght@400;700&display=swap");

                         body {
                             font-family: "Roboto", sans-serif;
                             background-color: var(--background-color);
                             margin: 0;
                             padding: 0;
                             color: var(--general-text-color);
                         }

                         :root {
                             --background-color: #f6f8fa;
                             --button-color: #b0b3b8;
                             --secondary-text-color: #a0a3ab;
                             --active-like-color: #8a8f99;
                             --icon-panel-color: #6a687e;
                             --general-text-color: #3a3a3a;
                         }

                         * {
                             scroll-behavior: smooth;
                         }

                         .container {
                             background-color: #ffffff;
                             max-width: 600px;
                             margin: 30px auto;
                             padding: 40px;
                             border-radius: 8px;
                             box-shadow: 0 4px 10px rgba(0,0,0,0.1);
                         }

                         .brand-header {
                             font-size: 40px;
                             font-weight: 700;
                             color: var(--general-text-color);
                             margin-bottom: 30px;
                             font-family: "Roboto", sans-serif;
                             text-align: left;
                         }

                         h1 {
                             color: var(--general-text-color);
                             font-weight: 700;
                             margin-bottom: 10px;
                             text-align: left;
                         }

                         p {
                             color: var(--secondary-text-color);
                             line-height: 1.6;
                             font-weight: 400;
                             text-align: left;
                             margin: 0 0 15px 0;
                         }

                         .btn-container {
                             text-align: center;
                             margin-top: 20px;
                         }

                         .btn {
                             display: inline-block;
                             padding: 12px 24px;
                             background-color: var(--button-color);
                             color: white;
                             text-decoration: none;
                             border-radius: 360px;
                             cursor: pointer;
                             font-family: "Roboto", sans-serif;
                             transition: transform 0.3s ease, background-color 0.3s ease;
                         }

                         .btn:hover {
                             background-color: var(--active-like-color);
                             transform: scale(1.1);
                         }

                         .footer {
                             margin-top: 40px;
                             font-size: 12px;
                             color: var(--secondary-text-color);
                             text-align: center;
                         }

                         a {
                             color: var(--secondary-text-color);
                             text-decoration: none;
                         }

                         a:hover {
                             text-decoration: none;
                         }
                     </style>
                 </head>
                 <body>
                     <div class="container">
                         <h1>Hello {{System.Net.WebUtility.HtmlEncode(firstName)}},</h1>
                         <p>Thanks for registering on Social Campus. To finish setting up your account, please verify your email by clicking the button below.</p>

                         <div class="btn-container">
                             <a href="{{System.Net.WebUtility.HtmlEncode(verificationLink)}}" class="btn">Verify Email</a>
                         </div>

                         <div class="footer">
                             <p>If you did not request this, you can safely ignore this email.</p>
                             <p>&copy; 2025 Social Campus. All rights reserved.</p>
                         </div>
                     </div>
                 </body>
                 </html>
                 """;
    }

    public static string GetResetPasswordHtml(string firstName, string resetLink)
    {
        return $$"""
                 <!DOCTYPE html>
                 <html lang="en">
                 <head>
                     <meta charset="UTF-8" />
                     <meta name="viewport" content="width=device-width, initial-scale=1.0" />
                     <title>Reset Your Password</title>
                     <style>
                         @import url("https://fonts.googleapis.com/css2?family=Roboto:wght@400;700&display=swap");

                         body {
                             font-family: "Roboto", sans-serif;
                             background-color: var(--background-color);
                             margin: 0;
                             padding: 0;
                             color: var(--general-text-color);
                         }

                         :root {
                             --background-color: #f6f8fa;
                             --button-color: #b0b3b8;
                             --secondary-text-color: #a0a3ab;
                             --active-like-color: #8a8f99;
                             --icon-panel-color: #6a687e;
                             --general-text-color: #3a3a3a;
                         }

                         * {
                             scroll-behavior: smooth;
                         }

                         .container {
                             background-color: #ffffff;
                             max-width: 600px;
                             margin: 30px auto;
                             padding: 40px;
                             border-radius: 8px;
                             box-shadow: 0 4px 10px rgba(0,0,0,0.1);
                         }

                         .brand-header {
                             font-size: 40px;
                             font-weight: 700;
                             color: var(--general-text-color);
                             margin-bottom: 30px;
                             font-family: "Roboto", sans-serif;
                             text-align: left;
                         }

                         h1 {
                             color: var(--general-text-color);
                             font-weight: 700;
                             margin-bottom: 10px;
                             text-align: left;
                         }

                         p {
                             color: var(--secondary-text-color);
                             line-height: 1.6;
                             font-weight: 400;
                             text-align: left;
                             margin: 0 0 15px 0;
                         }

                         .btn-container {
                             text-align: center;
                             margin-top: 20px;
                         }

                         .btn {
                             display: inline-block;
                             padding: 12px 24px;
                             background-color: var(--button-color);
                             color: white;
                             text-decoration: none;
                             border-radius: 360px;
                             cursor: pointer;
                             font-family: "Roboto", sans-serif;
                             transition: transform 0.3s ease, background-color 0.3s ease;
                         }

                         .btn:hover {
                             background-color: var(--active-like-color);
                             transform: scale(1.1);
                         }

                         .footer {
                             margin-top: 40px;
                             font-size: 12px;
                             color: var(--secondary-text-color);
                             text-align: center;
                         }

                         a {
                             color: var(--secondary-text-color);
                             text-decoration: none;
                         }

                         a:hover {
                             text-decoration: none;
                         }
                     </style>
                 </head>
                 <body>
                     <div class="container">
                         <h1>Hello {{System.Net.WebUtility.HtmlEncode(firstName)}},</h1>
                         <p>We received a request to reset your password for your Social Campus account. If this was you, click the button below to set a new password.</p>

                         <div class="btn-container">
                             <a href="{{System.Net.WebUtility.HtmlEncode(resetLink)}}" class="btn">Reset Password</a>
                         </div>

                         <div class="footer">
                             <p>If you didn’t request a password reset, you can safely ignore this email.</p>
                             <p>&copy; 2025 Social Campus. All rights reserved.</p>
                         </div>
                     </div>
                 </body>
                 </html>
                 """;
    }
}