import {
  IoHomeOutline,
  IoSearchOutline,
  IoMailOutline,
  IoPersonOutline,
  IoHome,
  IoSearch,
  IoMail,
  IoPerson,
} from "react-icons/io5";

const SidebarItems = [
  {
    path: "/home",
    label: "Home",
    inactiveIcon: IoHomeOutline,
    activeIcon: IoHome,
  },
  {
    path: "/search",
    label: "Search",
    inactiveIcon: IoSearchOutline,
    activeIcon: IoSearch,
  },
  {
    path: "/messages",
    label: "Messages",
    inactiveIcon: IoMailOutline,
    activeIcon: IoMail,
  },
  {
    path: "/profile/johndoe",
    label: "Profile",
    inactiveIcon: IoPersonOutline,
    activeIcon: IoPerson,
  },
];

export default SidebarItems;