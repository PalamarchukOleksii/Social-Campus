import React, { useState, useEffect } from "react";
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
  const [user, setUser] = useState({});
  const [sidebarItems, setSidebarItems] = useState([]);
  const { auth } = useAuth();

  const { openCreatePublication } = useCreateItem();

  useEffect(() => {
    const currentUser = auth?.shortUser || {};
    setUser(currentUser);
    setSidebarItems(GetSidebarItems(currentUser.login || ""));
  }, [auth]);

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
                    key={path}
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
          username={user.firstName + " " + user.lastName}
          login={user.login || ""}
          profileImage={
            user.profileImage
              ? `data:image/png;base64,${user.profileImage}`
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
