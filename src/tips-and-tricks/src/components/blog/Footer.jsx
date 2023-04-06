import styles from '../../styles/layout.module.css';

export default function Footer() {
	return (
		<footer className={`${styles.footer} border-top text-muted`}>
			<div className='container-fluid text-center bg-white'>
				&#169; 2023 - Cats & Tricks Blog
			</div>
		</footer>
	);
}
