import React, { useState } from "react";
import {
  IoHomeOutline,
  IoSearchOutline,
  IoChatboxEllipsesOutline,
  IoPersonOutline,
  IoExitOutline,
  IoHomeSharp,
  IoSearchSharp,
  IoChatboxEllipses,
  IoPersonSharp,
  IoExit,
} from "react-icons/io5";
import "./Sidebar.css";
import { useNavigate } from "react-router-dom";

function Sidebar() {
  const navigate = useNavigate();
  const [hoveredIcon, setHoveredIcon] = useState(null);

  const handleLogout = async (e) => {
    e.preventDefault();
    try {
      // Logout logic
      console.log("User logged out");
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
            <a
              href="/home"
              onMouseEnter={() => setHoveredIcon("home")}
              onMouseLeave={() => setHoveredIcon(null)}
            >
              {hoveredIcon === "home" ? (
                <IoHomeSharp className="icon" />
              ) : (
                <IoHomeOutline className="icon" />
              )}
              <span>Home</span>
            </a>
          </li>
          <li>
            <a
              href="/search"
              onMouseEnter={() => setHoveredIcon("search")}
              onMouseLeave={() => setHoveredIcon(null)}
            >
              {hoveredIcon === "search" ? (
                <IoSearchSharp className="icon" />
              ) : (
                <IoSearchOutline className="icon" />
              )}
              <span>Search</span>
            </a>
          </li>
          <li>
            <a
              href="/messages"
              onMouseEnter={() => setHoveredIcon("messages")}
              onMouseLeave={() => setHoveredIcon(null)}
            >
              {hoveredIcon === "messages" ? (
                <IoChatboxEllipses className="icon" />
              ) : (
                <IoChatboxEllipsesOutline className="icon" />
              )}
              <span>Messages</span>
            </a>
          </li>
          <li>
            <a
              href="/profile"
              onMouseEnter={() => setHoveredIcon("profile")}
              onMouseLeave={() => setHoveredIcon(null)}
            >
              {hoveredIcon === "profile" ? (
                <IoPersonSharp className="icon" />
              ) : (
                <IoPersonOutline className="icon" />
              )}
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
        <div className="profile-info">
          <h3 className="general-text">Username</h3>
          <h4 className="general-text">@login</h4>
        </div>
        <div
          onClick={handleLogout}
          className="logout-icon"
          onMouseEnter={() => setHoveredIcon("logout")}
          onMouseLeave={() => setHoveredIcon(null)}
        >
          {hoveredIcon === "logout" ? (
            <IoExit className="exit-icon" />
          ) : (
            <IoExitOutline className="exit-icon" />
          )}
        </div>
      </div>
    </div>
  );
}

export default Sidebar;
