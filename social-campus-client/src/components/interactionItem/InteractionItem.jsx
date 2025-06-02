import React, { useState } from "react";
import PropTypes from "prop-types";
import "./InteractionItem.css";

function InteractionItem(props) {
  const [isHovered, setIsHovered] = useState(false);

  const handleClick = () => {
    if (props.itemType === "comment" || props.itemType === "reply") {
      props.onClick();
    } else {
      props.onClick();
    }
  };

  return (
    <div
      className={`interaction-item ${props.className || ""}`}
      onClick={handleClick}
      onMouseEnter={() => setIsHovered(true)}
      onMouseLeave={() => setIsHovered(false)}
    >
      {isHovered && props.hoverIcon ? (
        <props.hoverIcon className="hover icon" />
      ) : props.isActive ? (
        props.activeIcon ? (
          <props.activeIcon className="active icon" />
        ) : (
          <props.icon className="icon" />
        )
      ) : (
        <props.icon className="icon" />
      )}
      <span className="stat">{props.label}</span>
    </div>
  );
}

InteractionItem.propTypes = {
  itemType: PropTypes.string.isRequired,
  label: PropTypes.number.isRequired,
  icon: PropTypes.elementType.isRequired,
  activeIcon: PropTypes.elementType,
  hoverIcon: PropTypes.elementType,
  onClick: PropTypes.func,
  isActive: PropTypes.bool,
  className: PropTypes.string,
};

export default InteractionItem;
