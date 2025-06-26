import {
  IoHomeOutline,
  IoSearchOutline,
  IoPersonOutline,
  IoHome,
  IoSearch,
  IoPerson,
} from "react-icons/io5";

const GetSidebarItems = (login) => [
  {
    path: "/home",
    label: "Home",
    inactiveIcon: IoHomeOutline,
    activeIcon: IoHome,
  },
  {
    path: "/search/users",
    label: "Search",
    inactiveIcon: IoSearchOutline,
    activeIcon: IoSearch,
  },
  {
    path: `/profile/${login}`,
    label: "Profile",
    inactiveIcon: IoPersonOutline,
    activeIcon: IoPerson,
  },
];

export default GetSidebarItems;
