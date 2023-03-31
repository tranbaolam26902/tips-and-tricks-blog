import { Link } from 'react-router-dom';

export default function TagList({ tags }) {
	return (
		<>
			{tags && Array.isArray(tags) && tags.length > 0
				? tags.map((tag, index) => (
						<Link
							key={index}
							to={`/blog/tag/${tag.urlSlug}`}
							title={tag.name}
							className='btn btn-sm btn-outline-secondary me-1'
						>
							{tag.name}
						</Link>
				  ))
				: null}
		</>
	);
}
