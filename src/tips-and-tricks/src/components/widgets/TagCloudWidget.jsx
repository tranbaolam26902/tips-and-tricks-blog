import { useState, useEffect } from 'react';
import { Link } from 'react-router-dom';

import { getTagCloud } from '../../services/widgets';

export default function TagCloudWidget() {
	// Component's states
	const [tags, setTags] = useState([]);

	useEffect(() => {
		fetchTags();

		async function fetchTags() {
			const data = await getTagCloud();
			if (data) setTags(data.items);
			else setTags([]);
		}
	}, []);

	return (
		<div className='mb-4'>
			<h3 className='mb-2 text-success'>Tag Cloud</h3>
			{tags.map((tag, index) => (
				<Link
					key={index}
					to={`/blog/tag/${tag.urlSlug}`}
					title={tag.name}
					className='btn btn-sm btn-outline-secondary me-2 mb-2'
				>
					{tag.name}
				</Link>
			))}
		</div>
	);
}
