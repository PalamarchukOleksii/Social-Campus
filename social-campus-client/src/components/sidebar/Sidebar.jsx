import React, { useState } from "react";
import "./Sidebar.css";
import { useNavigate } from "react-router-dom";
import { IoExit, IoExitOutline } from "react-icons/io5";
import NavItem from "../navItem/NavItem";
import ShortProfile from "../shortProfile/ShortProfile";
import NavItems from "../../utils/consts/NavItems";

function Sidebar() {
  const navigate = useNavigate();
  const [hoveredIcon, setHoveredIcon] = useState("");

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
        <img src="/android-chrome-512x512.png" alt="logo" />
        <span>Social Campus</span>
      </div>
      <div className="navigation">
        <ul>
          {NavItems.map(
            ({
              path,
              label,
              inactiveIcon: InactiveIcon,
              activeIcon: ActiveIcon,
            }) => {
              return (
                <NavItem
                  key={path}
                  path={path}
                  label={label}
                  inactiveIcon={InactiveIcon}
                  activeIcon={ActiveIcon}
                  hoveredIcon={hoveredIcon}
                  setHoveredIcon={setHoveredIcon}
                />
              );
            }
          )}
        </ul>
      </div>
      <div className="logout">
        <ShortProfile
          handleLogout={handleLogout}
          setHoveredIcon={setHoveredIcon}
          hoveredIcon={hoveredIcon}
        />
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
