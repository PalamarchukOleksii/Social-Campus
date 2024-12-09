import React from "react";
import SearchTabs from "../../components/searchTabs/SearchTabs";

function UsersSearch() {
  return (
    <div className="search-page-container">
      <input
        type="text"
        placeholder="Type to search..."
        className="search-input"
      />
      <SearchTabs />
    </div>
  );
}

export default UsersSearch;
