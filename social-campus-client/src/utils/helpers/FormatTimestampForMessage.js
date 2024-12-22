function FormatTimestampForMessage(timestamp) {
  const dateObj = new Date(timestamp);
  const today = new Date();

  const isToday =
    dateObj.getDate() === today.getDate() &&
    dateObj.getMonth() === today.getMonth() &&
    dateObj.getFullYear() === today.getFullYear();

  const time = dateObj.toLocaleTimeString([], {
    hour: "2-digit",
    minute: "2-digit",
  });

  if (isToday) {
    return time;
  }

  const date = dateObj.toLocaleDateString([], {
    month: "short",
    day: "numeric",
    year: "numeric",
  });
  return `${date}, ${time}`;
}

export default FormatTimestampForMessage;
