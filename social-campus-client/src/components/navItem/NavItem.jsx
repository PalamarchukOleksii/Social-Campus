import React from "react";
import "./NavItem.css";
import PropTypes from "prop-types";
import { Link, useLocation } from "react-router-dom";

function NavItem({
  activeIcon: ActiveIcon,
  inactiveIcon: InactiveIcon,
  label,
  path,
  onClick,
  setHoveredIcon,
  hoveredIcon,
  showLabel = true,
}) {
  const location = useLocation();
  const isActive = location.pathname === path;

  return (
    <div className="nav-item">
      <Link
        to={path}
        onMouseEnter={() => setHoveredIcon(label.toLowerCase())}
        onMouseLeave={() => setHoveredIcon("")}
        onClick={onClick}
        className={`link ${isActive ? "active" : ""}`}
      >
        <>
          {hoveredIcon === label.toLowerCase() || isActive ? (
            ActiveIcon ? (
              <ActiveIcon className="icon" />
            ) : null
          ) : InactiveIcon ? (
            <InactiveIcon className="icon" />
          ) : null}
          {showLabel && label && <span>{label}</span>}
        </>
      </Link>
    </div>
  );
}

NavItem.propTypes = {
  activeIcon: PropTypes.elementType,
  inactiveIcon: PropTypes.elementType,
  label: PropTypes.string,
  path: PropTypes.string,
  onClick: PropTypes.func,
  setHoveredIcon: PropTypes.func.isRequired,
  hoveredIcon: PropTypes.string.isRequired,
  showLabel: PropTypes.bool,
};

export default NavItem;
