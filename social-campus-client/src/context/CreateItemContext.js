import React, { createContext, useState, useContext } from "react";
import PropTypes from "prop-types";

const CreateItemContext = createContext();

export function CreateItemContextProvider({ children }) {
  const [isOpen, setIsOpen] = useState(false);

  const open = () => setIsOpen(true);
  const close = () => setIsOpen(false);

  return (
    <CreateItemContext.Provider value={{ isOpen, open, close }}>
      {children}
    </CreateItemContext.Provider>
  );
}

CreateItemContextProvider.propTypes = {
  children: PropTypes.node.isRequired,
};

export function useCreateItem() {
  return useContext(CreateItemContext);
}
