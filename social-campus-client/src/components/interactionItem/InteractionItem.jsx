import React from "react";
import PropTypes from "prop-types";
import "./InteractionItem.css";

function InteractionItem(props) {
  return (
    <div className="interaction-item">
      {props.icon && <props.icon className="icon" />}
      <span className="stat">{props.label}</span>
    </div>
  );
}

InteractionItem.propTypes = {
  label: PropTypes.string.isRequired,
  icon: PropTypes.elementType.isRequired,
};

export default InteractionItem;
