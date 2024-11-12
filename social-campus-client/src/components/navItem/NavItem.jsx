import React from "react";
import "./NavItem.css";
import PropTypes from "prop-types";
import { Link, useLocation } from "react-router-dom";

function NavItem(props) {
  const location = useLocation();
  const isActive = location.pathname === props.path;

  return (
    <div className="nav-item">
      <Link
        to={props.path}
        onMouseEnter={() => props.setHoveredIcon(props.label.toLowerCase())}
        onMouseLeave={() => props.setHoveredIcon("")}
        onClick={props.onClick}
        className={`link ${isActive ? "active" : ""}`}
      >
        <>
          {props.hoveredIcon === props.label.toLowerCase() || isActive ? (
            props.activeIcon ? (
              <props.activeIcon className="icon" />
            ) : null
          ) : props.inactiveIcon ? (
            <props.inactiveIcon className="icon" />
          ) : null}
          {props.label && <span>{props.label}</span>}
        </>
      </Link>
    </div>
  );
}

NavItem.propTypes = {
  activeIcon: PropTypes.elementType,
  inactiveIcon: PropTypes.elementType,
  label: PropTypes.string,
  path: PropTypes.string.isRequired,
  onClick: PropTypes.func,
  setHoveredIcon: PropTypes.func.isRequired,
  hoveredIcon: PropTypes.string.isRequired,
};

export default NavItem;
