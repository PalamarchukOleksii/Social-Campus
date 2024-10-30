import React from "react";
import { CiHome, CiSearch, CiChat2, CiUser } from "react-icons/ci";
import "./Sidebar.css";
import { useNavigate } from "react-router-dom";

function Sidebar() {
  const navigate = useNavigate();

  const handleLoggout = async (e) => {
    e.preventDefault();
    try {
      console.log("Sign up with Google");
      navigate("/");
    } catch (error) {
      console.log(error);
    }
  };

  return (
    <div className="sidebar">
      <div className="head">
        <img src="/android-chrome-512x512.png" alt="logo"></img>
        <span>Social Campus</span>
      </div>
      <div className="navigation">
        <ul>
          <li>
            <a href="/home">
              <CiHome className="icon" />
              <span>Home</span>
            </a>
          </li>
          <li>
            <a href="/search">
              <CiSearch className="icon" />
              <span>Search</span>
            </a>
          </li>
          <li>
            <a href="/messages">
              <CiChat2 className="icon" />
              <span>Messages</span>
            </a>
          </li>
          <li>
            <a href="/profile">
              <CiUser className="icon" />
              <span>Profile</span>
            </a>
          </li>
        </ul>
      </div>
      <div className="logout">
        <img
          src="https://via.placeholder.com/150"
          alt="Profile"
          className="profile-image"
        />
        <button className="loggout-btn" onClick={handleLoggout}>
          Logout
        </button>
      </div>
    </div>
  );
}

export default Sidebar;
