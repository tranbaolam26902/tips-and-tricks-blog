import SearchForm from './SearchForm';
import {
	ArchivesWidget,
	CategoriesWidget,
	FeaturedPostsWidget,
	RandomPostsWidget,
	TagCloudWidget,
	TopAuthorsWidget,
} from './widgets';

export default function Sidebar() {
	return (
		<div className='pt-4 ps-2'>
			<SearchForm />
			<CategoriesWidget />
			<FeaturedPostsWidget />
			<RandomPostsWidget />
			<TagCloudWidget />
			<TopAuthorsWidget />
			<ArchivesWidget />
			<h1>Đăng ký nhận tin mới</h1>
		</div>
	);
}
