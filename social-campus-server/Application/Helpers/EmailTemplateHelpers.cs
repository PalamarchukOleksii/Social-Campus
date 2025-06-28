using System.Net;

namespace Application.Helpers;

public static class EmailTemplateHelpers
{
    public static string GetVerifyEmailHtml(string email, string verificationLink)
    {
        return $"""
                <!DOCTYPE html>
                <html lang="en">
                <head>
                    <meta charset="UTF-8" />
                    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
                    <title>Verify Your Email</title>
                </head>
                <body>
                <div>
                    <h1>Verify your email</h1>
                    <p>To continue creating your account on Social Campus, please verify your email address by clicking the button below.</p>

                    <div>
                        <a href="{WebUtility.HtmlEncode(verificationLink)}" style="padding:12px 24px; background-color:#b0b3b8; color:white; text-decoration:none; border-radius: 360px; display:inline-block;">Verify Email</a>
                    </div>

                    <div style="margin-top:40px; font-size:12px; color:#a0a3ab; text-align:center;">
                        <p>If you didn't request this, you can safely ignore it.</p>
                        <p>&copy; {WebUtility.HtmlEncode(DateTime.Now.Year.ToString())} Social Campus. All rights reserved.</p>
                    </div>
                </div>
                </body>
                </html>
                """;
    }

    public static string GetResetPasswordHtml(string firstName, string resetLink)
    {
        return $"""
                <!DOCTYPE html>
                <html lang="en">
                <head>
                    <meta charset="UTF-8" />
                    <meta name="viewport" content="width=device-width, initial-scale=1.0" />
                    <title>Reset Your Password</title>
                </head>
                <body>
                    <div>
                        <h1>Hello {WebUtility.HtmlEncode(firstName)},</h1>
                        <p>We received a request to reset your password for your Social Campus account. If this was you, click the button below to set a new password.</p>

                        <div>
                            <a href="{WebUtility.HtmlEncode(resetLink)}" style="padding:12px 24px; background-color:#b0b3b8; color:white; text-decoration:none; border-radius: 360px; display:inline-block;">Reset Password</a>
                        </div>

                        <div style="margin-top:40px; font-size:12px; color:#a0a3ab; text-align:center;">
                            <p>If you didn’t request a password reset, you can safely ignore this email.</p>
                            <p>&copy; {WebUtility.HtmlEncode(DateTime.Now.Year.ToString())} Social Campus. All rights reserved.</p>
                        </div>
                    </div>
                </body>
                </html>
                """;
    }
}