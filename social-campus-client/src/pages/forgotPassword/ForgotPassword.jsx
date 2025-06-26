import React, { useState, useEffect } from "react";
import { useNavigate, useSearchParams } from "react-router-dom";
import "./ForgotPassword.css";
import { toast } from "react-toastify";
import axios from "../../utils/api/AxiosBase";
import Loading from "../../components/loading/Loading";

const GENERATE_RESET_TOKEN_REQUEST_URL = "/api/reset-password-tokens/generate";
const RESET_USER_PASSWORD_URL = "/api/users/reset-password";

function ForgotPassword() {
  const [searchParams] = useSearchParams();
  const navigate = useNavigate();

  const token = searchParams.get("token");
  const userId = searchParams.get("userId");
  const errorMsg = searchParams.get("errorMsg");

  const [email, setEmail] = useState("");
  const [newPassword, setNewPassword] = useState("");
  const [newPasswordConfirm, setNewPasswordConfirm] = useState("");
  const [showPassword, setShowPassword] = useState(false);
  const [loading, setLoading] = useState(false);

  let isResetStep = !!token && !!userId;

  useEffect(() => {
    if (!isResetStep && errorMsg) {
      toast.error(errorMsg);
    }
  }, [isResetStep, errorMsg]);

  useEffect(() => {
    if (token && userId) {
      isResetStep = true;
    }
  }, [token, userId]);

  const handleSendResetLink = async () => {
    if (!email) return toast.error("Email is required");
    setLoading(true);
    try {
      await axios.post(GENERATE_RESET_TOKEN_REQUEST_URL, { email: email });
      toast.success("Reset link sent to your email.");
    } catch (err) {
      const { response } = err;
      toast.error(response?.data?.detail || "Something went wrong.");
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  const handleResetPassword = async () => {
    if (newPassword !== newPasswordConfirm) {
      toast.error("Passwords do not match");
      return;
    }

    setLoading(true);
    try {
      await axios.post(RESET_USER_PASSWORD_URL, {
        resetPasswordToken: token,
        userId: userId,
        newPassword: newPassword,
      });
      toast.success("Password updated successfully");
      navigate("/signin");
    } catch (err) {
      const { response } = err;
      toast.error(response?.data?.detail || "Something went wrong.");
      console.error(err);
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="forgot-password-container">
      <div className="forgot-password-left-panel">
        <img
          src="/android-chrome-512x512.png"
          alt="Logo"
          className="forgot-password-logo"
        />
      </div>
      <div className="forgot-password-right-panel">
        <div className="forgot-password-form-card">
          <div className="forgot-password-form-content">
            <h1 className="forgot-password-title general-text">
              {isResetStep ? "Reset Password" : "Forgot Password"}
            </h1>

            {loading ? (
              <div className="forgot-password-loading-wrapper">
                <Loading />
              </div>
            ) : (
              <>
                {isResetStep ? (
                  <>
                    <input
                      type={showPassword ? "text" : "password"}
                      placeholder="New password"
                      value={newPassword}
                      onChange={(e) => setNewPassword(e.target.value)}
                      className="forgot-password-input"
                    />
                    <input
                      type={showPassword ? "text" : "password"}
                      placeholder="Confirm new password"
                      value={newPasswordConfirm}
                      onChange={(e) => setNewPasswordConfirm(e.target.value)}
                      className="forgot-password-input"
                    />
                    <label className="forgot-password-show-password-label not-general-text">
                      <input
                        type="checkbox"
                        checked={showPassword}
                        onChange={() => setShowPassword(!showPassword)}
                        className="forgot-password-checkbox"
                      />
                      Show password
                    </label>
                    <button
                      onClick={handleResetPassword}
                      className="forgot-password-button"
                    >
                      Reset Password
                    </button>
                  </>
                ) : (
                  <>
                    <input
                      type="email"
                      placeholder="Enter your email"
                      value={email}
                      onChange={(e) => setEmail(e.target.value)}
                      className="forgot-password-input"
                    />
                    <div>
                      <button
                        onClick={handleSendResetLink}
                        className="forgot-password-button"
                      >
                        Send Reset Link
                      </button>
                    </div>
                  </>
                )}
              </>
            )}
          </div>
        </div>
      </div>
    </div>
  );
}

export default ForgotPassword;
