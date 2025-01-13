import React, { useState } from "react";
import "./Sidebar.css";
import { useNavigate } from "react-router-dom";
import { IoExit, IoExitOutline } from "react-icons/io5";
import NavItem from "../navItem/NavItem";
import GetSidebarItems from "../../utils/consts/GetSidebarItems";
import ShortProfile from "../shortProfile/ShortProfile";
import { useCreateItem } from "../../context/CreateItemContext";
import useAuth from "../../hooks/useAuth";

function Sidebar() {
  const navigate = useNavigate();
  const [hoveredIcon, setHoveredIcon] = useState("");
  const { auth } = useAuth();

  const { openCreatePublication } = useCreateItem();

  const handleLogout = async (e) => {
    e.preventDefault();
    try {
      navigate("/");
    } catch (error) {
      console.error(error);
    }
  };

  const handlePublishClick = () => {
    openCreatePublication();
  };

  const sidebarItems = GetSidebarItems(auth.shortUser.login);

  return (
    <div className="sidebar">
      <div className="wrapper">
        <div className="head">
          <img src="/android-chrome-512x512.png" alt="logo" />
          <span>Social Campus</span>
        </div>
        <div className="navigation">
          <ul>
            {sidebarItems.map(
              ({
                path,
                label,
                inactiveIcon: InactiveIcon,
                activeIcon: ActiveIcon,
              }) => (
                <li key={path}>
                  <NavItem
                    path={path}
                    label={label}
                    inactiveIcon={InactiveIcon}
                    activeIcon={ActiveIcon}
                    hoveredIcon={hoveredIcon}
                    setHoveredIcon={setHoveredIcon}
                  />
                </li>
              )
            )}
          </ul>
          <div className="button-wrapper">
            <button className="add-publish" onClick={handlePublishClick}>
              Publish
            </button>
          </div>
        </div>
      </div>
      <div className="logout">
        <ShortProfile
          username={auth.shortUser.firstName + " " + auth.shortUser.lastName}
          login={auth.shortUser.login}
          profileImage={
            auth.shortUser.profileImage
              ? `data:image/png;base64,${auth.shortUser.profileImage}`
              : null
          }
        />
        <div
          onClick={handleLogout}
          className="logout-icon"
          onMouseEnter={() => setHoveredIcon("logout")}
          onMouseLeave={() => setHoveredIcon("")}
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
