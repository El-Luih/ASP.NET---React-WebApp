//HAMBURGER REUSABLE BUTTON
type HamburgerButtonProps = {
    isOpen: boolean; 
    setIsOpen: (isOpen: boolean) => void;
    ariaControlsId: string;
    menuClassName: string;
}
export default function HamburgerButton({
    /**Initializes open state. */
    isOpen,

    /**Function to set "isOpen" boolean value */
    setIsOpen,

    /**Specifies the controlled dropdown menu id*/
    ariaControlsId,

    /**Specifies the semantic name of the controlled menu */
    menuClassName,
}: HamburgerButtonProps) {

    return (
        <button
            type="button"
            className={`hamburger-btn ${menuClassName.toLowerCase()}-menu ${isOpen ? "open" : ""}`}
            onClick={() => setIsOpen(!isOpen)}
            aria-expanded={isOpen}
            aria-controls={ariaControlsId}
            aria-label={isOpen ? `Close ${menuClassName} menu` : `Open ${menuClassName} menu`}
            >
        </button>
    )
}