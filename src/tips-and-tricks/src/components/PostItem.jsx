import { Card } from 'react-bootstrap';
import { Link } from 'react-router-dom';

import { isEmptyOrWhitespace } from '../utils';
import TagList from './TagList';

export default function PostItem({ post }) {
	// Variables
	const imageUrl = isEmptyOrWhitespace(post.imageUrl)
		? process.env.PUBLIC_URL + '/assets/images/default-thumbnail.jpg'
		: `${post.imageUrl}`;
	const postedDate = new Date(post.postedDate);

	return (
		<article className='blog-entry mb-4'>
			<Card>
				<div className='row g-0'>
					<div className='col-md-4'>
						<Card.Img
							variant='top'
							src={imageUrl}
							alt={post.title}
						/>
					</div>
					<div className='col-md-8'>
						<Card.Body>
							<Card.Title>
								<Link
									to={`/blog/post/${postedDate.getFullYear()}/${postedDate.getMonth()}/${postedDate.getDate()}/${
										post.urlSlug
									}`}
									className='text-decoration-none'
								>
									{post.title}
								</Link>
							</Card.Title>
							<Card.Text>
								<small className='text-muted'>Tác giả:</small>
								<Link
									to={`/blog/author/${post.author.urlSlug}`}
									className='text-primary text-decoration-none m-1'
								>
									{post.author.fullName}
								</Link>
								<small className='text-muted'>Chủ đề:</small>
								<Link
									to={`blog/category/${post.category.urlSlug}`}
									className='text-primary text-decoration-none m-1'
								>
									{post.category.name}
								</Link>
							</Card.Text>
							<Card.Text>{post.shortDescription}</Card.Text>
							<div className='tag-list'>
								<TagList tags={post.tags} />
							</div>
							<div className='text-end'>
								<Link
									to={`/blog/post?year=${postedDate.getFullYear()}&month=${postedDate.getMonth()}&day=${postedDate.getDate()}&slug=${
										post.urlSlug
									}`}
									title={post.title}
									className='btn btn-primary'
								>
									Xem chi tiết
								</Link>
							</div>
						</Card.Body>
					</div>
				</div>
			</Card>
		</article>
	);
}
