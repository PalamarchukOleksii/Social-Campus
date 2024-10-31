import React from "react";
import "./NavItem.css";
import PropTypes from "prop-types";

function NavItem(props) {
  return (
    <div className="nav-item">
      <li>
        <a
          href={props.path}
          onMouseEnter={() => props.setHoveredIcon(props.label.toLowerCase())}
          onMouseLeave={() => props.setHoveredIcon(null)}
        >
          {props.hoveredIcon === props.label.toLowerCase() ? (
            <props.activeIcon className="icon" />
          ) : (
            <props.inactiveIcon className="icon" />
          )}
          <span>{props.label}</span>
        </a>
      </li>
    </div>
  );
}

NavItem.propTypes = {
  activeIcon: PropTypes.string.isRequired,
  inactiveIcon: PropTypes.string.isRequired,
  label: PropTypes.string.isRequired,
  path: PropTypes.string.isRequired,
  setHoveredIcon: PropTypes.func.isRequired,
  hoveredIcon: PropTypes.string.isRequired,
};

export default NavItem;
