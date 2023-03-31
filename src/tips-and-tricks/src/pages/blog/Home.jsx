import { useState, useEffect } from 'react';
import { useParams } from 'react-router-dom';

import PostsFilter from '../../components/PostsFilter';

export default function Home() {
	useEffect(() => {
		document.title = 'Trang chủ';
	}, []);

	return (
		<div className='p-4'>
			<PostsFilter postQuery={{}} />
		</div>
	);
}
