import React from "react";
import "./TagsSearch.css";
import SearchTabs from "../../components/searchTabs/SearchTabs";

function TagsSearch() {
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

export default TagsSearch;
