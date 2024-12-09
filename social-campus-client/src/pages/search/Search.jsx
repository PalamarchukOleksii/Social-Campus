import React from "react";
import "./Search.css";
import SearchTabs from "../../components/searchTabs/SearchTabs";

function Search() {
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

export default Search;
