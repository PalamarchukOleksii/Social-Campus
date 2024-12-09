import React, { useState, useEffect } from "react";
import "./TagsSearch.css";
import Tag from "../../components/tag/Tag";
import jsonData from "../../data/userData.json";
import SearchTabs from "../../components/searchTabs/SearchTabs";

function TagsSearch() {
  const [searchTerm, setSearchTerm] = useState("");
  const [tags, setTags] = useState([]);

  useEffect(() => {
    const extractTags = () => {
      const tagCountMap = new Map();

      jsonData.forEach((user) => {
        user.publications.forEach((publication) => {
          const regex = /#\w+/g;
          const matches = publication.description.match(regex);
          if (matches) {
            matches.forEach((tag) => {
              tagCountMap.set(tag, (tagCountMap.get(tag) || 0) + 1);
            });
          }
        });
      });

      const tagsWithCount = Array.from(tagCountMap, ([tag, count]) => ({
        tag,
        count,
      }));
      setTags(tagsWithCount);
    };

    extractTags();
  }, []);

  const filteredTags = tags.filter((tag) =>
    tag.tag.toLowerCase().includes(searchTerm.toLowerCase())
  );

  return (
    <div className="search-page-container">
      <input
        type="text"
        placeholder="Type to search for tags..."
        className="search-input"
        value={searchTerm}
        onChange={(e) => setSearchTerm(e.target.value)}
      />
      <SearchTabs />
      <div className="tags-container">
        {filteredTags.map((tagObj, index) => (
          <Tag key={index} tagName={tagObj.tag} postsCount={tagObj.count} />
        ))}
      </div>
    </div>
  );
}

export default TagsSearch;
