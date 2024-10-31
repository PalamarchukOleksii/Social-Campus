import React, { useState } from "react";
import "./Sidebar.css";
import { useNavigate, useLocation } from "react-router-dom";
import NavItem from "../navItem/NavItem";
import ShortProfile from "../shortProfile/ShortProfile";
import NavItems from "../../utils/consts/NavItems";

function Sidebar() {
  const navigate = useNavigate();
  const location = useLocation();
  const currentPath = location.pathname;
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
              const isActive = currentPath === path;

              return (
                <NavItem
                  key={path}
                  path={path}
                  label={label}
                  inactiveIcon={InactiveIcon}
                  activeIcon={ActiveIcon}
                  hoveredIcon={hoveredIcon}
                  setHoveredIcon={setHoveredIcon}
                  isActive={isActive}
                />
              );
            }
          )}
        </ul>
      </div>
      <ShortProfile
        handleLogout={handleLogout}
        setHoveredIcon={setHoveredIcon}
        hoveredIcon={hoveredIcon}
      />
    </div>
  );
}

export default Sidebar;
