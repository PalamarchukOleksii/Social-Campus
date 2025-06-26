import React, { useState, useEffect } from "react";
import { useNavigate, useSearchParams } from "react-router-dom";
import "./SignUp.css";
import { toast } from "react-toastify";
import axios from "../../utils/api/AxiosBase";
import Loading from "../../components/loading/Loading";

const VERIFY_EMAIL_URL = "/api/verify-email-tokens/generate";
const REGISTER_URL = "/api/users/register";

function SignUp() {
  const [searchParams] = useSearchParams();

  const token = searchParams.get("token");
  const linkEmail = searchParams.get("email");
  const errorMsg = searchParams.get("errorMsg");

  useEffect(() => {
    if (!token && errorMsg) {
      toast.error(errorMsg);
    }
  }, [token, errorMsg]);

  useEffect(() => {
    if (token && linkEmail) {
      setStep(2);
    }
  }, [token, linkEmail]);

  const [step, setStep] = useState(1);
  const [loading, setLoading] = useState(false);
  const [email, setEmail] = useState("");
  const [login, setLogin] = useState("");
  const [firstName, setFirstName] = useState("");
  const [lastName, setLastName] = useState("");
  const [password, setPassword] = useState("");
  const [confirmPassword, setConfirmPassword] = useState("");
  const [showPassword, setShowPassword] = useState(false);

  const navigate = useNavigate();

  const handleVerifyEmail = async () => {
    if (!email) return toast.error("Email is required");

    setLoading(true);
    try {
      await axios.post(VERIFY_EMAIL_URL, { email: email });
      toast.success("Verification email sent.");
    } catch (err) {
      const { response } = err;
      toast.error(
        response?.data?.detail || "Failed to send verification email."
      );
    } finally {
      setLoading(false);
    }
  };

  const handleNextInfo = () => {
    if (!login || !firstName || !lastName) {
      toast.error("All fields are required.");
      return;
    }
    setStep(3);
  };

  const handleRegister = async () => {
    if (password !== confirmPassword) {
      toast.error("Passwords do not match.");
      return;
    }

    setLoading(true);
    try {
      await axios.post(REGISTER_URL, {
        login: login,
        firstName: firstName,
        lastName: lastName,
        email: linkEmail,
        password: password,
        verifyEmailToken: token,
      });
      toast.success("Account created successfully!");
      navigate("/signin");
    } catch (err) {
      const { response } = err;
      toast.error(response?.data?.detail || "Registration failed.");
    } finally {
      setLoading(false);
    }
  };

  return (
    <div className="signup-container">
      <div className="signup-left-panel">
        <img
          src="/android-chrome-512x512.png"
          alt="Logo"
          className="signup-logo"
        />
      </div>
      <div className="signup-right-panel">
        <div className="signup-form-card">
          <div className="signup-form-content">
            <h1 className="signup-title general-text">Sign Up</h1>
            {loading ? (
              <div className="signup-loading-wrapper">
                <Loading />
              </div>
            ) : (
              <>
                {step === 1 && (
                  <>
                    <input
                      type="email"
                      placeholder="Enter your email"
                      value={email}
                      onChange={(e) => setEmail(e.target.value)}
                      className="signup-input"
                    />
                    <button
                      onClick={handleVerifyEmail}
                      className="signup-button"
                    >
                      Verify Email
                    </button>
                  </>
                )}
                {step === 2 && (
                  <>
                    <input
                      type="text"
                      placeholder="Login"
                      value={login}
                      onChange={(e) => setLogin(e.target.value)}
                      className="signup-input"
                    />
                    <input
                      type="text"
                      placeholder="First Name"
                      value={firstName}
                      onChange={(e) => setFirstName(e.target.value)}
                      className="signup-input"
                    />
                    <input
                      type="text"
                      placeholder="Last Name"
                      value={lastName}
                      onChange={(e) => setLastName(e.target.value)}
                      className="signup-input"
                    />
                    <button onClick={handleNextInfo} className="signup-button">
                      Next
                    </button>
                  </>
                )}
                {step === 3 && (
                  <>
                    <input
                      type={showPassword ? "text" : "password"}
                      placeholder="Password"
                      value={password}
                      onChange={(e) => setPassword(e.target.value)}
                      className="signup-input"
                    />
                    <input
                      type={showPassword ? "text" : "password"}
                      placeholder="Confirm Password"
                      value={confirmPassword}
                      onChange={(e) => setConfirmPassword(e.target.value)}
                      className="signup-input"
                    />
                    <label className="signup-show-password-label not-general-text">
                      <input
                        type="checkbox"
                        checked={showPassword}
                        onChange={() => setShowPassword(!showPassword)}
                        className="signup-checkbox"
                      />
                      Show password
                    </label>
                    <button onClick={handleRegister} className="signup-button">
                      Register
                    </button>
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

export default SignUp;
