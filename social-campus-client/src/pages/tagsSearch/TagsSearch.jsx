import React, { useState, useEffect, useRef } from "react";
import "./TagsSearch.css";
import Tag from "../../components/tag/Tag";
import SearchTabs from "../../components/searchTabs/SearchTabs";
import useAxiosPrivate from "../../hooks/useAxiosPrivate";
import Loading from "../../components/loading/Loading";

const SEARCH_TAGS_BASE_URL = "/api/tags/searchterm";
const PAGE_SIZE = 10;

function TagsSearch() {
  const axios = useAxiosPrivate();

  const [searchQuery, setSearchQuery] = useState("");
  const [searchResultTags, setSearchResultTags] = useState([]);
  const [loading, setLoading] = useState(false);
  const [allFetched, setAllFetched] = useState(false);
  const [page, setPage] = useState(1);

  const debounceTimeoutRef = useRef(null);

  const fetchTags = async (reset = false, pageNum = 1) => {
    try {
      setLoading(true);

      const response = await axios.get(
        `${SEARCH_TAGS_BASE_URL}/${searchQuery}/count/${PAGE_SIZE}/page/${pageNum}`
      );
      const tags = response.data;

      if (reset) {
        setSearchResultTags(tags);
      } else {
        setSearchResultTags((prev) => [...prev, ...tags]);
      }

      setPage(pageNum + 1);
      setAllFetched(tags.length < PAGE_SIZE);
    } catch (error) {
      console.error("Error fetching tags:", error);
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    if (debounceTimeoutRef.current) clearTimeout(debounceTimeoutRef.current);

    if (searchQuery.trim().length === 0) {
      setSearchResultTags([]);
      setAllFetched(false);
      setPage(1);
      setLoading(false);
      return;
    }

    debounceTimeoutRef.current = setTimeout(() => {
      fetchTags(true, 1);
    }, 250);

    return () => {
      if (debounceTimeoutRef.current) clearTimeout(debounceTimeoutRef.current);
    };
  }, [searchQuery, axios]);

  return (
    <div className="search-page-container">
      <input
        type="text"
        placeholder="Type to search for tags..."
        className="tag-search-input"
        value={searchQuery}
        onChange={(e) => {
          const filtered = e.target.value.replace(
            /[^a-zA-Zа-яА-ЯіІїЇєЄґҐ0-9]/g,
            ""
          );
          setSearchQuery(filtered);
        }}
      />
      <SearchTabs />
      <div className="tags-container">
        {loading && searchResultTags.length === 0 ? (
          <Loading />
        ) : searchResultTags.length > 0 ? (
          searchResultTags.map((tag, index) => (
            <Tag
              key={index}
              tagName={tag.label}
              postsCount={tag.publicationsCount}
            />
          ))
        ) : (
          <h2 className="not-found-users general-text">No tags found.</h2>
        )}
      </div>
      {!allFetched && !loading && searchResultTags.length > 0 && (
        <div className="load-more-container">
          <button onClick={() => fetchTags(false, page)}>Load More</button>
        </div>
      )}
    </div>
  );
}

export default TagsSearch;
