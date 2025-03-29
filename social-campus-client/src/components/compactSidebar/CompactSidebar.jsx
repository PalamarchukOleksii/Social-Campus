import React, { useEffect, useState } from "react";
import "./CompactSidebar.css";
import { useNavigate } from "react-router-dom";
import { IoExit, IoExitOutline } from "react-icons/io5";
import NavItem from "../navItem/NavItem";
import { IoAdd } from "react-icons/io5";
import useAuth from "../../hooks/useAuth";
import GetSidebarItems from "../../utils/consts/GetSidebarItems";
import useLogout from "../../hooks/useLogout";
import ROUTES from "../../utils/consts/Routes";
import CreatePublication from "../createPublication/CreatePublication";

function CompactSidebar() {
  const navigate = useNavigate();
  const [hoveredIcon, setHoveredIcon] = useState("");
  const [sidebarItems, setSidebarItems] = useState([]);
  const [isCreatePublicationOpen, setIsCreatePublicationOpen] = useState(false);

  const { auth } = useAuth();
  const logout = useLogout();

  useEffect(() => {
    const currentUser = auth?.shortUser || {};
    setSidebarItems(GetSidebarItems(currentUser.login || ""));
  }, [auth]);

  const handleLogout = async (e) => {
    e.preventDefault();
    try {
      await logout();
      navigate(ROUTES.LANDING);
    } catch (error) {
      console.error(error);
    }
  };

  const handleOpenCreatePublishClick = () => {
    setIsCreatePublicationOpen(true);
  };

  const handleCloseCreatePublishClick = () => {
    setIsCreatePublicationOpen(false);
  };

  return (
    <div className="compact-sidebar">
      {isCreatePublicationOpen && (
        <div className="create-publication-modal-overlay">
          <CreatePublication onCloseClick={handleCloseCreatePublishClick} />
        </div>
      )}
      <div className="wrapper">
        <div className="head">
          <img src="/android-chrome-512x512.png" alt="logo" />
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
                    showLabel={false}
                  />
                </li>
              )
            )}
          </ul>
          <div className="button-wrapper">
            <button
              className="add-publish-short"
              onClick={handleOpenCreatePublishClick}
            >
              <IoAdd />
            </button>
          </div>
        </div>
      </div>
      <div className="compact-logout">
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

export default CompactSidebar;
