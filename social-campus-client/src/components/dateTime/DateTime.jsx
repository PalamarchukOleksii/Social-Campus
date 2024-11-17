import React, { useState, useEffect } from "react";
import PropTypes from "prop-types";
import "./DateTime.css";

function DateTime(props) {
  const [currentTime, setCurrentTime] = useState(new Date());

  useEffect(() => {
    const interval = setInterval(() => {
      setCurrentTime(new Date());
    }, 300000);

    return () => clearInterval(interval);
  }, []);

  const date = new Date(props.dateTime);
  const now = currentTime;

  const timeDifference = now - date;

  if (isNaN(date)) {
    return <span className="creation-time not-general-text">Invalid date</span>;
  }

  const adjustedTimeDifference = Math.max(timeDifference, 0);

  const hoursDifference = Math.floor(adjustedTimeDifference / (1000 * 60 * 60));
  const minutesDifference = Math.floor(adjustedTimeDifference / (1000 * 60));

  let displayDate = "";

  if (minutesDifference < 60) {
    displayDate = `${minutesDifference} minutes ago`;
  } else if (hoursDifference < 24) {
    displayDate = `${hoursDifference} hours ago`;
  } else {
    const currentYear = now.getFullYear();
    const yearOfDate = date.getFullYear();
    if (yearOfDate === currentYear) {
      displayDate = date.toLocaleString(props.locale, {
        month: "short",
        day: "numeric",
      });
    } else {
      displayDate = date.toLocaleString(props.locale, {
        month: "short",
        day: "numeric",
        year: "numeric",
      });
    }
  }

  return <span className="creation-time not-general-text">{displayDate}</span>;
}

DateTime.propTypes = {
  dateTime: PropTypes.oneOfType([PropTypes.string, PropTypes.instanceOf(Date)])
    .isRequired,
  locale: PropTypes.string,
};

export default DateTime;
