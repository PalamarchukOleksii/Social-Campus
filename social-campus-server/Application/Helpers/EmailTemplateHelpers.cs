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
                        <a href="{WebUtility.HtmlEncode(verificationLink)}">Verify Email</a>
                    </div>

                    <div>
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
                            <a href="{WebUtility.HtmlEncode(resetLink)}">Reset Password</a>
                        </div>

                        <div>
                            <p>If you didn’t request a password reset, you can safely ignore this email.</p>
                            <p>&copy; {WebUtility.HtmlEncode(DateTime.Now.Year.ToString())} Social Campus. All rights reserved.</p>
                        </div>
                    </div>
                </body>
                </html>
                """;
    }
}